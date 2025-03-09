using Auto.Common.Entities.Authentication;
using Auto.Common.Enums;
using Auto.Database;
using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Common.Package;
using Notio.Cryptography.Hash;
using Notio.Network.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auto.Server.Services;

public sealed class AccountService(AutoDbContext context) : BaseService
{
    private readonly AutoDbContext context = context;
    private const int iterations = 100_000;
    private const int keyLength = 32;

    /// <summary>
    /// Xử lý đăng ký tài khoản mới
    /// </summary>
    [PacketCommand((int)Command.RegisterAccount, Authoritys.Guests)]
    public void RegisterAccount(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 2, out string[] parts))
        {
            // Không tiết lộ lỗi format
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        string username = parts[0];
        string password = parts[1];

        // Kiểm tra username đã tồn tại chưa (ẩn lỗi)
        if (context.Accounts.Any(a => a.Username == username))
        {
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        // Tạo salt ngẫu nhiên
        byte[] salt = Notio.Randomization.RandGenerator.CreateKey(32);
        using var pbkdf2 = new Pbkdf2(salt, iterations, keyLength, Pbkdf2.HashAlgorithmType.Sha256);

        Account newAccount = new()
        {
            Username = username,
            PasswordHash = $"{Convert.ToHexString(salt)}:{Convert.ToHexString(pbkdf2.DeriveKey(password))}",
            Role = Authoritys.User,
            CreatedAt = DateTime.UtcNow
        };

        context.Accounts.Add(newAccount);
        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Account registered successfully."));
    }

    /// <summary>
    /// Xử lý đăng nhập người dùng
    /// </summary>
    [PacketCommand((int)Command.Login, Authoritys.Guests)]
    public void Login(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 2, out string[] parts))
        {
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        string username = parts[0];
        string password = parts[1];

        // Truy vấn tài khoản từ database
        Account? account = context.Accounts.SingleOrDefault(a => a.Username == username);

        if (account == null || account.FailedLoginAttempts >= 5 && account.LastFailedLogin.AddMinutes(15) > DateTime.UtcNow)
        {
            connection.Send(CreateErrorPacket("Too many failed login attempts. Try again later."));
            return;
        }

        // Kiểm tra tài khoản có bị vô hiệu hóa không
        if (!account.IsActive)
        {
            connection.Send(CreateErrorPacket("Account is disabled."));
            return;
        }

        // Tách salt và hash
        parts = account.PasswordHash.Split(':');
        if (parts.Length != 2)
        {
            connection.Send(CreateErrorPacket("Invalid password format."));
            return;
        }

        // Hash lại mật khẩu nhập vào
        using var pbkdf2 = new Pbkdf2(Convert.FromHexString(parts[0]), iterations, keyLength, Pbkdf2.HashAlgorithmType.Sha256);
        byte[] hashToVerify = pbkdf2.DeriveKey(password);

        // Kiểm tra mật khẩu
        if (!Pbkdf2.ConstantTimeEquals(hashToVerify, Convert.FromHexString(parts[1])))
        {
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        // Cập nhật thời gian đăng nhập gần nhất
        account.LastLogin = DateTime.UtcNow;
        context.SaveChanges();

        // Lưu thông tin đăng nhập vào connection
        connection.Authority = account.Role;
        connection.Metadata["Username"] = account.Username;
        connection.Send(CreateSuccessPacket("Login successful."));
    }

    /// <summary>
    /// Xóa tài khoản (Chỉ dành cho Admin)
    /// </summary>
    [PacketCommand((int)Command.DeleteAccount, Authoritys.Administrator)]
    public void DeleteAccount(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 1, out string[] parts) || !int.TryParse(parts[0], out int accountId))
        {
            connection.Send(CreateErrorPacket("Invalid account ID."));
            return;
        }

        // Tìm tài khoản cần xóa
        Account? account = context.Accounts.Find(accountId);
        if (account == null)
        {
            connection.Send(CreateErrorPacket("Account not found."));
            return;
        }

        context.Accounts.Remove(account);
        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Account deleted successfully."));
    }

    /// <summary>
    /// Cập nhật mật khẩu tài khoản (Chỉ cho chính chủ)
    /// </summary>
    [PacketCommand((int)Command.UpdatePassword, Authoritys.User)]
    public void UpdatePassword(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 3, out string[] parts))
        {
            connection.Send(CreateErrorPacket("Invalid data format."));
            return;
        }

        string username = parts[0];
        string oldPassword = parts[1];
        string newPassword = parts[2];

        // Kiểm tra độ dài mật khẩu mới
        if (newPassword.Length < 8)
        {
            connection.Send(CreateErrorPacket("New password must be at least 8 characters long."));
            return;
        }

        // Tìm tài khoản cần đổi mật khẩu
        Account? account = context.Accounts.SingleOrDefault(a => a.Username == username);
        if (account == null)
        {
            connection.Send(CreateErrorPacket("Account not found."));
            return;
        }

        // Kiểm tra người dùng có quyền thay đổi mật khẩu này không
        if (connection.Metadata.GetValueOrDefault("Username") is not string sessionUsername
            || account.Username != sessionUsername)
        {
            connection.Send(CreateErrorPacket("You are not allowed to change this password."));
            return;
        }

        // Tách salt và hash của mật khẩu hiện tại
        string[] hashParts = account.PasswordHash.Split(':');
        if (hashParts.Length != 2)
        {
            connection.Send(CreateErrorPacket("Invalid password format."));
            return;
        }

        // Hash lại mật khẩu cũ để kiểm tra
        byte[] salt = Convert.FromHexString(hashParts[0]);
        using var pbkdf2 = new Pbkdf2(salt, iterations, keyLength, Pbkdf2.HashAlgorithmType.Sha256);
        byte[] oldPasswordHash = pbkdf2.DeriveKey(oldPassword);

        if (!Pbkdf2.ConstantTimeEquals(oldPasswordHash, Convert.FromHexString(hashParts[1])))
        {
            connection.Send(CreateErrorPacket("Incorrect old password."));
            return;
        }

        // Cập nhật mật khẩu mới
        byte[] newPasswordHash = pbkdf2.DeriveKey(newPassword);
        account.PasswordHash = $"{Convert.ToHexString(salt)}:{Convert.ToHexString(newPasswordHash)}";

        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Password updated successfully."));
    }
}
