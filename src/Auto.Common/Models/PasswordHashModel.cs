namespace Auto.Common.Models;

/// <summary>
/// Lớp mô hình cho dữ liệu hash mật khẩu
/// </summary>
public sealed class PasswordHashModel
{
    public required string Salt { get; init; }
    public required string Hash { get; init; }
}
