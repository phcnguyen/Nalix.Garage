using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities;

/// <summary>
/// Entity đại diện cho tài khoản người dùng trong hệ thống.
/// </summary>
[Table(nameof(Account))]
public class Account
{
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
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Mật khẩu đã được băm (hashed password).
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string PasswordHash { get; private set; } = string.Empty;

    /// <summary>
    /// Ngày tạo tài khoản.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Ngày cập nhật cuối cùng.
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}