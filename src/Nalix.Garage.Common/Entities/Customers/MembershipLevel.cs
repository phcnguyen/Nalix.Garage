using System.ComponentModel.DataAnnotations;

namespace Nalix.Garage.Common.Entities.Customers;

/// <summary>
/// Enum đại diện cho cấp độ thành viên trong hệ thống.
/// </summary>
public enum MembershipLevel : byte
{
    [Display(Name = "Không xác định / Chưa đăng ký")]
    None = 0,

    [Display(Name = "Khách dùng thử")]
    Trial = 1,

    [Display(Name = "Khách thường")]
    Standard = 2,

    [Display(Name = "Thành viên bạc")]
    Silver = 3,

    [Display(Name = "Thành viên vàng")]
    Gold = 4,

    [Display(Name = "Thành viên bạch kim")]
    Platinum = 5,

    [Display(Name = "Thành viên kim cương")]
    Diamond = 6
}