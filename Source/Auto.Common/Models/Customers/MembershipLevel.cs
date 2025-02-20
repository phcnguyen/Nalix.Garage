namespace Auto.Common.Models.Customers;

/// <summary>
/// Enum đại diện cho cấp độ thành viên.
/// </summary>
public enum MembershipLevel
{
    /// <summary>
    /// Không xác định hoặc chưa đăng ký.
    /// </summary>
    None = 0,

    /// <summary>
    /// Khách thường.
    /// </summary>
    Standard = 1,

    /// <summary>
    /// Thành viên bạc.
    /// </summary>
    Silver = 2,

    /// <summary>
    /// Thành viên vàng.
    /// </summary>
    Gold = 3,

    /// <summary>
    /// Thành viên cao cấp.
    /// </summary>
    Platinum = 4
}