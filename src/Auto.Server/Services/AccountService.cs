using Auto.Common.Entities.Authentication;
using Auto.Common.Enums;
using Auto.Common.Models;
using Auto.Database;
using Auto.Server.Services.Base;
using Notio.Common.Attributes;
using Notio.Common.Authentication;
using Notio.Common.Connection;
using Notio.Common.Interfaces;
using Notio.Cryptography.Hash;
using Notio.Network.Package.Enums;
using Notio.Serialization;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Auto.Server.Services;

public sealed class AccountService(AutoDbContext context) : BaseService
{
    private const int SaltSize = 32;
    private const int KeyLength = 32;
    private const int Iterations = 50_000;

    private readonly AutoDbContext _context = context;

    /// <summary>
    /// Xử lý đăng ký tài khoản mới
    /// </summary>
    [PacketCommand((int)Command.RegisterAccount, Authoritys.Guest)]
    public void RegisterAccount(IPacket packet, IConnection connection)
    {
        string username, password;

        if (packet.Type == (byte)PacketType.String)
        {
            if (!TryParsePayload(packet, 2, out string[] parts) ||
                string.IsNullOrWhiteSpace(parts[0]) ||
                string.IsNullOrWhiteSpace(parts[1]) ||
                parts[0].Length < 3 || parts[1].Length < 8)
            {
                connection.Send(CreateErrorPacket("Invalid username or password."));
                return;
            }

            username = parts[0];
            password = parts[1];
        }
        else if (packet.Type == (byte)PacketType.Json)
        {
            Account acc = Json.Deserialize<Account>(packet.Payload.Span);
            username = acc.Username;
            password = acc.PasswordHash;
        }
        else
        {
            connection.Send(InvalidDataPacket());
            return;
        }

        // Kiểm tra username đã tồn tại chưa
        if (_context.Accounts.Any(a => a.Username == username))
        {
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        // Hash mật khẩu
        string passwordHash = HashPassword(password);

        Account newAccount = new()
        {
            Username = username,
            PasswordHash = passwordHash,
            Role = Authoritys.User,
            CreatedAt = DateTime.UtcNow
        };

        _context.Accounts.Add(newAccount);
        _context.SaveChanges();
        connection.Send(CreateSuccessPacket("Account registered successfully."));
    }

    /// <summary>
    /// Xử lý đăng nhập người dùng
    /// </summary>
    [PacketCommand((int)Command.Login, Authoritys.Guest)]
    public void Login(IPacket packet, IConnection connection)
    {
        string username, password;

        if (packet.Type == (byte)PacketType.String)
        {
            if (!TryParsePayload(packet, 2, out string[] parts))
            {
                connection.Send(CreateErrorPacket("Invalid username or password."));
                return;
            }
            username = parts[0];
            password = parts[1];
        }
        else if (packet.Type == (byte)PacketType.Json)
        {
            Account acc = Json.Deserialize<Account>(packet.Payload.Span);
            username = acc.Username;
            password = acc.PasswordHash;
        }
        else
        {
            connection.Send(InvalidDataPacket());
            return;
        }

        Account? account = _context.Accounts.FirstOrDefault(a => a.Username == username);

        if (account == null || !VerifyPassword(password, account.PasswordHash))
        {
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        if (!account.IsActive)
        {
            connection.Send(CreateErrorPacket("Account is disabled."));
            return;
        }

        // Cập nhật thông tin tài khoản
        account.LastLogin = DateTime.UtcNow;
        _context.SaveChanges();

        // Cập nhật phiên đăng nhập
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
        int accountId;

        if (packet.Type == (byte)PacketType.String)
        {
            if (!TryParsePayload(packet, 1, out string[] parts) || !int.TryParse(parts[0], out accountId))
            {
                connection.Send(CreateErrorPacket("Invalid account ID."));
                return;
            }
        }
        else if (packet.Type == (byte)PacketType.Json)
        {
            accountId = Json.Deserialize<Account>(packet.Payload.Span).Id;
        }
        else
        {
            connection.Send(InvalidDataPacket());
            return;
        }

        Account? account = _context.Accounts.FirstOrDefault(a => a.Id == accountId);
        if (account == null)
        {
            connection.Send(CreateErrorPacket("Account not found."));
            return;
        }

        _context.Accounts.Remove(account);
        _context.SaveChanges();
        connection.Send(CreateSuccessPacket("Account deleted successfully."));
    }

    /// <summary>
    /// Cập nhật mật khẩu tài khoản (Chỉ cho chính chủ)
    /// </summary>
    [PacketCommand((int)Command.UpdatePassword, Authoritys.User)]
    public void UpdatePassword(IPacket packet, IConnection connection)
    {
        string username, oldPassword, newPassword;

        if (packet.Type == (byte)PacketType.String)
        {
            if (!TryParsePayload(packet, 3, out string[] parts))
            {
                connection.Send(InvalidDataPacket());
                return;
            }

            username = parts[0];
            oldPassword = parts[1];
            newPassword = parts[2];
        }
        else
        {
            connection.Send(InvalidDataPacket());
            return;
        }

        if (newPassword.Length < 8)
        {
            connection.Send(CreateErrorPacket("New password must be at least 8 characters long."));
            return;
        }

        Account? account = _context.Accounts.FirstOrDefault(a => a.Username == username);
        if (account == null)
        {
            connection.Send(CreateErrorPacket("Account not found."));
            return;
        }

        if (!connection.Metadata.TryGetValue("Username", out object? value) ||
            value is not string sessionUsername ||
            account.Username != sessionUsername)
        {
            connection.Send(CreateErrorPacket("You are not allowed to change this password."));
            return;
        }

        if (!VerifyPassword(oldPassword, account.PasswordHash))
        {
            connection.Send(CreateErrorPacket("Incorrect old password."));
            return;
        }

        // Tạo salt mới cho mật khẩu mới
        account.PasswordHash = HashPassword(newPassword);
        _context.SaveChanges();
        connection.Send(CreateSuccessPacket("Password updated successfully."));
    }

    /// <summary>
    /// Tạo hash mật khẩu sử dụng PBKDF2
    /// </summary>
    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        using var pbkdf2 = new Pbkdf2(salt, Iterations, KeyLength, Pbkdf2.HashAlgorithmType.Sha256);
        byte[] hash = pbkdf2.DeriveKey(password);

        return Json.Serialize(new PasswordHashModel
        {
            Salt = Convert.ToHexString(salt),
            Hash = Convert.ToHexString(hash)
        }, false, null);
    }

    /// <summary>
    /// Kiểm tra mật khẩu nhập vào có khớp với hash hay không
    /// </summary>
    private static bool VerifyPassword(string password, string storedHash)
    {
        try
        {
            // Deserialize JSON
            PasswordHashModel data = Json.Deserialize<PasswordHashModel>(storedHash);

            // Chuyển đổi về dạng byte
            byte[] salt = Convert.FromHexString(data.Salt);
            byte[] storedKey = Convert.FromHexString(data.Hash);

            // Tạo key mới từ mật khẩu nhập vào
            using var pbkdf2 = new Pbkdf2(salt, Iterations, KeyLength, Pbkdf2.HashAlgorithmType.Sha256);
            byte[] computedKey = pbkdf2.DeriveKey(password);

            // So sánh theo thời gian cố định để tránh timing attack
            return CryptographicOperations.FixedTimeEquals(computedKey, storedKey);
        }
        catch
        {
            return false; // Trả về false nếu có lỗi khi parse dữ liệu
        }
    }
}
