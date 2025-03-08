using Auto.Common.Entities.Authentication;
using Auto.Common.Enums;
using Auto.Database;
using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Common.Package;
using Notio.Cryptography.Hash;
using Notio.Network.Handlers;
using System;
using System.Linq;

namespace Auto.Server.Services;

public sealed class AccountService(AutoDbContext context) : BaseService
{
    private readonly AutoDbContext context = context;
    private const int iterations = 100_000;
    private const int keyLength = 32;

    [PacketCommand((int)Command.RegisterAccount, Authoritys.Guests)]
    public void RegisterAccount(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 2, out string[] parts))
        {
            connection.Send(CreateErrorPacket("Invalid data format."));
            return;
        }

        string username = parts[0];
        string password = parts[1];

        if (context.Accounts.Any(a => a.Username == username))
        {
            connection.Send(CreateErrorPacket("Username already exists."));
            return;
        }

        byte[] salt = Notio.Randomization.RandGenerator.CreateKey(32);

        using var pbkdf2 = new Pbkdf2(salt, iterations, keyLength, Pbkdf2.HashAlgorithmType.Sha256);

        Account newAccount = new()
        {
            Username = username,
            PasswordHash = $"{Convert.ToHexString(salt)}:{pbkdf2.DeriveKey(password)}",
            Role = Authoritys.User,
            CreatedAt = DateTime.UtcNow
        };

        context.Accounts.Add(newAccount);
        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Account registered successfully."));
    }

    [PacketCommand((int)Command.Login, Authoritys.Guests)]
    public void Login(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 2, out string[] parts))
        {
            connection.Send(CreateErrorPacket("Invalid data format."));
            return;
        }

        string username = parts[0];
        string password = parts[1];

        Account? account = context.Accounts.FirstOrDefault(a => a.Username == username);

        if (account == null)
        {
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        if (!account.IsActive)
        {
            connection.Send(CreateErrorPacket("Account is disabled."));
            return;
        }

        parts = account.PasswordHash.Split(':');
        using var pbkdf2 = new Pbkdf2(
            Convert.FromHexString(parts[0]), iterations, keyLength, Pbkdf2.HashAlgorithmType.Sha256);

        // Hash mật khẩu người dùng nhập
        byte[] hashToVerify = pbkdf2.DeriveKey(password);

        if (!Pbkdf2.ConstantTimeEquals(hashToVerify, Convert.FromHexString(parts[1])))
        {
            connection.Send(CreateErrorPacket("Invalid username or password."));
            return;
        }

        account.LastLogin = DateTime.UtcNow;
        context.SaveChanges();

        connection.Authority = account.Role;
        connection.Send(CreateSuccessPacket("Login successful."));
    }

    [PacketCommand((int)Command.DeleteAccount, Authoritys.Administrator)]
    public void DeleteAccount(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 1, out string[] parts) || !int.TryParse(parts[0], out int accountId))
        {
            connection.Send(CreateErrorPacket("Invalid account ID."));
            return;
        }

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

    [PacketCommand((int)Command.UpdatePassword, Authoritys.User)]
    public void UpdatePassword(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 2, out string[] parts))
        {
            connection.Send(CreateErrorPacket("Invalid data format."));
            return;
        }

        string username = parts[0];
        string newPassword = parts[1];

        Account? account = context.Accounts.FirstOrDefault(a => a.Username == username);
        if (account == null)
        {
            connection.Send(CreateErrorPacket("Account not found."));
            return;
        }

        byte[] salt = Notio.Randomization.RandGenerator.CreateKey(32);
        using var pbkdf2 = new Pbkdf2(salt, iterations, keyLength, Pbkdf2.HashAlgorithmType.Sha256);

        account.PasswordHash = $"{Convert.ToHexString(salt)}:{pbkdf2.DeriveKey(newPassword)}";
        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Password updated successfully."));
    }
}
