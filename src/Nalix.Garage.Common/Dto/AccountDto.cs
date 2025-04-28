namespace Nalix.Garage.Common.Dto;

/// <summary>
/// Mô hình tài khoản người dùng, chứa thông tin cơ bản về tài khoản.
/// </summary>
public class AccountDto
{
    /// <summary>
    /// Khóa chính của tài khoản (ID). 
    /// Được tự động tạo khi tài khoản được thêm vào cơ sở dữ liệu.
    /// </summary>
    public int Id { get; set; } = -1;

    /// <summary>
    /// Tên đăng nhập của tài khoản.
    /// Dùng để xác định danh tính người dùng trong hệ thống.
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// Mật khẩu của tài khoản. 
    /// Khi lưu trữ, mật khẩu cần được băm (hashed) để đảm bảo bảo mật.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
