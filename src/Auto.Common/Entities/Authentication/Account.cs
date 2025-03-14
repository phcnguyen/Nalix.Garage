using Notio.Common.Attributes;
using Notio.Common.Authentication;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Authentication;

/// <summary>
/// Entity đại diện cho tài khoản người dùng trong hệ thống.
/// </summary>
[Table(nameof(Account))]
public class Account
{
    #region Fields

    private string _username;
    private string _password;

    #endregion

    #region Identification Properties

    /// <summary>
    /// Khóa chính (ID tài khoản, tự động tạo).
    /// </summary>
    [Key]
    [JsonInclude]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên đăng nhập (username). Dùng để đăng nhập hệ thống.
    /// </summary>
    [Required]
    [JsonInclude]
    [MaxLength(50)]
    [RegularExpression(@"^[a-zA-Z0-9_-]+$",
        ErrorMessage = "Username can only contain letters, numbers, underscores, and hyphens.")]
    public string Username
    {
        get => _username;
        set => _username = value?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Mật khẩu đã được băm (hashed password).
    /// </summary>
    [Required]
    [MaxLength(512)]
    [Column(TypeName = "char(128)")]
    [JsonInclude]
    [JsonProperty("Password")]
    public string PasswordHash
    {
        get => _password;
        set => _password = value?.Trim() ?? string.Empty;
    }

    #endregion

    #region Role and Status Properties

    /// <summary>
    /// Vai trò của tài khoản trong hệ thống.
    /// </summary>
    [Required]
    public Authoritys Role { get; set; } = Authoritys.Guest;

    /// <summary>
    /// Trạng thái hoạt động của tài khoản.
    /// </summary>
    public bool IsActive { get; set; } = true;

    #endregion

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

    #endregion

    #region Audit Properties

    /// <summary>
    /// Ngày tạo tài khoản.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

    #endregion
}