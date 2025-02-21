using System.ComponentModel;

namespace Auto.Common.Entites.Customers;

/// <summary>
/// Enum đại diện cho cấp độ thành viên trong hệ thống.
/// </summary>
public enum MembershipLevel : byte
{
    [Description("Không xác định / Chưa đăng ký")]
    None = 0,

    [Description("Khách dùng thử")]
    Trial = 1,

    [Description("Khách thường")]
    Standard = 2,

    [Description("Thành viên bạc")]
    Silver = 3,

    [Description("Thành viên vàng")]
    Gold = 4,

    [Description("Thành viên bạch kim")]
    Platinum = 5,

    [Description("Thành viên kim cương (VIP)")]
    Diamond = 6
}