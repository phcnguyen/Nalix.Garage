using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entities.Customers;

/// <summary>
/// Enum đại diện cho loại khách hàng trong hệ thống.
/// </summary>
public enum CustomerType : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    [Display(Name = "Khách hàng cá nhân")]
    Individual = 1,

    [Display(Name = "Doanh nghiệp")]
    Business = 2,

    [Display(Name = "Cơ quan chính phủ")]
    Government = 3,

    [Display(Name = "Khách hàng sở hữu nhiều xe")]
    Fleet = 4,

    [Display(Name = "Công ty bảo hiểm")]
    InsuranceCompany = 5,

    [Display(Name = "Khách hàng VIP")]
    VIP = 6,

    [Display(Name = "Khách hàng tiềm năng")]
    Potential = 7,

    [Display(Name = "Nhà cung cấp")]
    Supplier = 8,

    [Display(Name = "Tổ chức phi lợi nhuận")]
    NonProfit = 9,

    [Display(Name = "Đại lý")]
    Dealer = 10,

    [Display(Name = "Loại khách hàng khác")]
    Other = 255
}
