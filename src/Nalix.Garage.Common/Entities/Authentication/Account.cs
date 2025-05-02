using Nalix.Common.Security;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nalix.Garage.Common.Entities.Authentication;

/// <summary>
/// Entity đại diện cho tài khoản người dùng trong hệ thống.
/// </summary>
[Table(nameof(Account))]
public class Account
{
    #region Fields

    private string _username;

    #endregion Fields

    #region Identification Properties

    /// <summary>
    /// Khóa chính (ID tài khoản, tự động tạo).
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên đăng nhập (username). Dùng để đăng nhập hệ thống.
    /// </summary>
    [Required]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$",
        ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens.")]
    public string Username
    {
        get => _username;
        set => _username = value?.Trim().ToLower() ?? string.Empty;
    }

    /// <summary>
    /// Chuỗi salt ngẫu nhiên được tạo ra để băm mật khẩu.
    /// Salt giúp bảo vệ mật khẩu khỏi các cuộc tấn công từ điển và rainbow table.
    /// </summary>
    [Required]
    [MaxLength(64)]
    [Column(TypeName = "binary(64)")]
    public byte[] Salt { get; set; }

    /// <summary>
    /// Mật khẩu sau khi được băm bằng thuật toán PBKDF2.
    /// Giá trị này được lưu trữ trong cơ sở dữ liệu để xác minh mật khẩu khi đăng nhập.
    /// </summary>
    [Required]
    [MaxLength(64)]
    [Column(TypeName = "binary(64)")]
    public byte[] Hash { get; set; }

    #endregion Identification Properties

    #region Role and Status Properties

    /// <summary>
    /// Vai trò của tài khoản trong hệ thống.
    /// </summary>
    [Required]
    public PermissionLevel Role { get; set; } = PermissionLevel.Guest;

    /// <summary>
    /// Trạng thái hoạt động của tài khoản.
    /// </summary>
    public bool IsActive { get; set; } = true;

    #endregion Role and Status Properties

    #region Login Tracking Properties

    /// <summary>
    /// Số lần đăng nhập thất bại.
    /// </summary>
    [Required]
    public byte FailedLoginAttempts { get; set; } = 0;

    /// <summary>
    /// Thời gian đăng nhập thất bại gần nhất.
    /// </summary>
    public DateTime? LastFailedLogin { get; set; }

    /// <summary>
    /// Thời gian đăng nhập gần nhất.
    /// </summary>
    public DateTime? LastLogin { get; set; }

    #endregion Login Tracking Properties

    #region Audit Properties

    /// <summary>
    /// Ngày tạo tài khoản.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    #endregion Audit Properties
}