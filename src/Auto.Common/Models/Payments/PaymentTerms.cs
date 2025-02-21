using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Payments;

/// <summary>
/// Enum đại diện cho các điều khoản thanh toán.
/// </summary>
public enum PaymentTerms : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    [Display(Name = "Thanh toán ngay khi nhận hàng")]
    DueOnReceipt = 1,

    [Display(Name = "Thanh toán trong 7 ngày")]
    Net7 = 2,

    [Display(Name = "Thanh toán trong 15 ngày")]
    Net15 = 3,

    [Display(Name = "Thanh toán trong 30 ngày")]
    Net30 = 4,

    [Display(Name = "Thỏa thuận riêng")]
    Custom = 255
}