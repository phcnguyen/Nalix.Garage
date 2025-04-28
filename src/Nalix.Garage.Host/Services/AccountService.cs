using Nalix.Garage.Common.Dto;
using Nalix.Garage.Common.Entities.Authentication;
using Nalix.Garage.Common.Enums;
using Nalix.Garage.Database;
using Nalix.Garage.Database.Repositories;
using Notio.Common.Attributes;
using Notio.Common.Connection;
using Notio.Common.Package;
using Notio.Common.Security;
using Notio.Cryptography.Security;
using Notio.Logging;
using Notio.Utilities;
using System;
using System.Threading.Tasks;

namespace Nalix.Garage.Host.Services;

/// <summary>
/// Dịch vụ quản lý tài khoản người dùng, bao gồm đăng ký, đăng nhập, xóa tài khoản và cập nhật mật khẩu.
/// </summary>
/// <remarks>
/// Khởi tạo AccountService với DbContext.
/// </remarks>
/// <param name="context">Context của cơ sở dữ liệu để thao tác với bảng Accounts.</param>
public sealed class AccountService(AutoDbContext context) : BaseService
{
    private readonly Repository<Account> _accountRepository = new(context);

    /// <summary>
    /// Đăng ký tài khoản mới cho người dùng.
    /// Định dạng dữ liệu:
    /// - String: "{username}:{password}" (phân tách bằng dấu hai chấm)
    /// - JSON: AccountDto { Username, Password }
    /// Yêu cầu: Username >= 3 ký tự, Password >= 8 ký tự.
    /// </summary>
    /// <param name="packet">Gói dữ liệu chứa thông tin đăng ký.</param>
    /// <param name="connection">Kết nối với client để gửi phản hồi.</param>
    /// <returns>Task đại diện cho quá trình xử lý bất đồng bộ.</returns>
    [PacketId((int)Command.RegisterAccount)]
    [PacketPermission(PermissionLevel.User)]
    public async Task RegisterAccountAsync(IPacket packet, IConnection connection)
    {
        var (isValid, username, password) = await ValidatePacketAsync(packet, connection);
        if (!isValid) return;

        if (await _accountRepository.AnyAsync(a => a.Username == username))
        {
            CLogging.Instance.Warn($"Username {username} already exists from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Username already existed."));
            return;
        }

        try
        {
            PasswordSecurity.HashPassword(password, out byte[] salt, out byte[] hash);
            Account newAccount = new()
            {
                Username = username,
                Salt = salt,
                Hash = hash,
                Role = PermissionLevel.User,
                CreatedAt = DateTime.UtcNow
            };

            _accountRepository.Add(newAccount);
            await _accountRepository.SaveChangesAsync();
            CLogging.Instance.Info($"Account {username} registered successfully from connection {connection.Id}");
            await connection.SendAsync(CreateSuccessPacket("Account registered successfully."));
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to register account {username} from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to register account due to an internal error."));
        }
    }

    /// <summary>
    /// Đăng nhập người dùng vào hệ thống.
    /// Định dạng dữ liệu:
    /// - String: "{username}:{password}" (phân tách bằng dấu hai chấm)
    /// - JSON: AccountDto { Username, Password }
    /// </summary>
    /// <param name="packet">Gói dữ liệu chứa thông tin đăng nhập.</param>
    /// <param name="connection">Kết nối với client để gửi phản hồi và cập nhật phiên.</param>
    /// <returns>Task đại diện cho quá trình xử lý bất đồng bộ.</param>
    [PacketId((int)Command.LoginAccount)]
    [PacketPermission(PermissionLevel.User)]
    public async Task LoginAsync(IPacket packet, IConnection connection)
    {
        var (isValid, username, password) = await ValidatePacketAsync(packet, connection);
        if (!isValid) return;

        Account? account = await _accountRepository.GetFirstOrDefaultAsync(a => a.Username == username);
        if (account == null)
        {
            CLogging.Instance.Warn($"LoginAccount attempt with non-existent username {username} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Username does not exist."));
            return;
        }

        if (account.FailedLoginAttempts >= 5 && account.LastFailedLogin.HasValue &&
            DateTime.UtcNow < account.LastFailedLogin.Value.AddMinutes(15))
        {
            CLogging.Instance.Warn($"Account {username} locked due to too many failed attempts from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Account locked due to too many failed attempts."));
            return;
        }

        if (!PasswordSecurity.VerifyPassword(password, account.Salt, account.Hash))
        {
            account.FailedLoginAttempts++;
            account.LastFailedLogin = DateTime.UtcNow;
            await _accountRepository.SaveChangesAsync();
            CLogging.Instance.Warn($"Incorrect password for {username}, attempt {account.FailedLoginAttempts} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Incorrect password."));
            return;
        }

        if (!account.IsActive)
        {
            CLogging.Instance.Warn($"LoginAccount attempt on disabled account {username} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Account is disabled."));
            return;
        }

        try
        {
            account.FailedLoginAttempts = 0;
            account.LastLogin = DateTime.UtcNow;
            await _accountRepository.SaveChangesAsync();

            connection.Level = account.Role;
            connection.Metadata["Username"] = account.Username;
            CLogging.Instance.Info($"User {username} logged in successfully from connection {connection.Id}");
            await connection.SendAsync(CreateSuccessPacket("LoginAccount successful."));
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to complete login for {username} from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to login due to an internal error."));
        }
    }

    /// <summary>
    /// Xóa tài khoản người dùng (chỉ dành cho quản trị viên).
    /// Định dạng dữ liệu:
    /// - String: "{accountId}"
    /// - JSON: Account { Id }
    /// </summary>
    /// <param name="packet">Gói dữ liệu chứa ID tài khoản cần xóa.</param>
    /// <param name="connection">Kết nối với client để gửi phản hồi.</param>
    /// <returns>Task đại diện cho quá trình xử lý bất đồng bộ.</returns>
    [PacketId((int)Command.DeleteAccount)]
    [PacketPermission(PermissionLevel.User)]
    public async Task DeleteAccountAsync(IPacket packet, IConnection connection)
    {
        int accountId;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 1, out string[] parts) || !int.TryParse(parts[0], out accountId))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid account ID."));
                return;
            }
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        Account? account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null)
        {
            CLogging.Instance.Warn($"Attempt to delete non-existent account ID {accountId} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Account not found."));
            return;
        }

        try
        {
            _accountRepository.Delete(account);
            await _accountRepository.SaveChangesAsync();
            CLogging.Instance.Info($"Account ID {accountId} (Username: {account.Username}) deleted successfully by connection {connection.Id}");
            await connection.SendAsync(CreateSuccessPacket("Account deleted successfully."));
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to delete account ID {accountId} from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to delete account due to an internal error."));
        }
    }

    /// <summary>
    /// Cập nhật mật khẩu tài khoản dựa trên phiên đăng nhập hiện tại (chỉ cho chính chủ).
    /// Định dạng dữ liệu:
    /// - String: "{oldPassword}:{newPassword}" (phân tách bằng dấu hai chấm)
    /// - JSON: PasswordChangeDto { OldPassword, NewPassword }
    /// Yêu cầu: NewPassword >= 8 ký tự.
    /// </summary>
    /// <param name="packet">Gói dữ liệu chứa mật khẩu cũ và mới.</param>
    /// <param name="connection">Kết nối với client để xác thực và gửi phản hồi.</param>
    /// <returns>Task đại diện cho quá trình xử lý bất đồng bộ.</returns>
    [PacketId((int)Command.UpdatePassword)]
    [PacketPermission(PermissionLevel.User)]
    public async Task UpdatePasswordAsync(IPacket packet, IConnection connection)
    {
        string oldPassword, newPassword;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 2, out string[] parts) ||
                string.IsNullOrWhiteSpace(parts[0]) ||
                string.IsNullOrWhiteSpace(parts[1]))
            {
                await connection.SendAsync(InvalidDataPacket());
                return;
            }
            oldPassword = parts[0];
            newPassword = parts[1];
        }
        else if (packet.Type == PacketType.Json)
        {
            PasswordChangeDto? acc = JsonBuffer.DeserializeFromBytes(
                packet.Payload.Span, JsonContext.Default.PasswordChangeDto);

            if (acc == null ||
                string.IsNullOrWhiteSpace(acc.OldPassword) ||
                string.IsNullOrWhiteSpace(acc.NewPassword))
            {
                await connection.SendAsync(InvalidDataPacket());
                return;
            }
            oldPassword = acc.OldPassword;
            newPassword = acc.NewPassword;
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        if (newPassword.Length < 8 || newPassword.Length > 128)
        {
            await connection.SendAsync(CreateErrorPacket("New password must be 8-128 characters long."));
            return;
        }

        if (!connection.Metadata.TryGetValue("Username", out object? value) || value is not string sessionUsername)
        {
            CLogging.Instance.Warn($"Unauthorized password update attempt from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("You are not allowed to change this password."));
            return;
        }

        Account? account = await _accountRepository.GetFirstOrDefaultAsync(a => a.Username == sessionUsername);
        if (account == null)
        {
            CLogging.Instance.Warn($"Account not found for username {sessionUsername} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Account not found."));
            return;
        }

        if (!PasswordSecurity.VerifyPassword(oldPassword, account.Salt, account.Hash))
        {
            CLogging.Instance.Warn($"Incorrect old password for {sessionUsername} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Incorrect old password."));
            return;
        }

        try
        {
            PasswordSecurity.HashPassword(newPassword, out byte[] salt, out byte[] hash);
            account.Salt = salt;
            account.Hash = hash;
            await _accountRepository.SaveChangesAsync();
            CLogging.Instance.Info($"Password updated successfully for {sessionUsername} from connection {connection.Id}");
            await connection.SendAsync(CreateSuccessPacket("Password updated successfully."));
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to update password for {sessionUsername} from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to update password due to an internal error."));
        }
    }

    private static async Task<(bool isValid, string username, string password)>
        ValidatePacketAsync(IPacket packet, IConnection connection)
    {
        string username, password;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 2, out string[] parts) ||
                string.IsNullOrWhiteSpace(parts[0]) ||
                string.IsNullOrWhiteSpace(parts[1]) ||
                parts[0].Length < 3 || parts[0].Length > 50 ||
                parts[1].Length < 8 || parts[1].Length > 128)
            {
                await connection.SendAsync(CreateErrorPacket("Invalid username or password."));
                return (false, string.Empty, string.Empty);
            }

            username = parts[0];
            password = parts[1];
        }
        else if (packet.Type == PacketType.Json)
        {
            AccountDto? acc = JsonBuffer.DeserializeFromBytes(
                packet.Payload.Span, JsonContext.Default.AccountDto);

            if (acc == null ||
                string.IsNullOrWhiteSpace(acc.Username) ||
                string.IsNullOrWhiteSpace(acc.Password) ||
                acc.Username.Length < 3 || acc.Username.Length > 50 ||
                acc.Password.Length < 8 || acc.Password.Length > 128)
            {
                await connection.SendAsync(CreateErrorPacket("Invalid username or password."));
                return (false, string.Empty, string.Empty);
            }

            username = acc.Username;
            password = acc.Password;
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return (false, string.Empty, string.Empty);
        }

        return (true, username, password);
    }
}