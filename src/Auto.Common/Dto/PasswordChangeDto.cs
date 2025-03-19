namespace Auto.Common.Dto;

/// <summary>
/// Mô hình chứa thông tin mật khẩu khi thay đổi hoặc xác thực.
/// </summary>
public class PasswordChangeDto
{
    /// <summary>
    /// Mật khẩu hiện tại hoặc mật khẩu cần xác thực.
    /// </summary>
    public string OldPassword { get; set; }

    /// <summary>
    /// Mật khẩu mới (chỉ sử dụng khi cập nhật mật khẩu).
    /// </summary>
    public string NewPassword { get; set; }
}
