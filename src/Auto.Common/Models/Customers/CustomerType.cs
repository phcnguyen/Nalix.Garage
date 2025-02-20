using System.ComponentModel;

namespace Auto.Common.Models.Customers;

/// <summary>
/// Enum đại diện cho loại khách hàng trong hệ thống.
/// </summary>
public enum CustomerType : byte
{
    [Description("Không xác định")]
    None = 0,

    [Description("Khách hàng cá nhân")]
    Individual = 1,

    [Description("Doanh nghiệp")]
    Business = 2,

    [Description("Cơ quan chính phủ")]
    Government = 3,

    [Description("Khách hàng sở hữu nhiều xe")]
    Fleet = 4,

    [Description("Công ty bảo hiểm")]
    InsuranceCompany = 5,

    [Description("Khách hàng VIP")]
    VIP = 6,

    [Description("Loại khách hàng khác")]
    Other = 255
}