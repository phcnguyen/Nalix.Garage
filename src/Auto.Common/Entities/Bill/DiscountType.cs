using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entities.Bill;

/// <summary>
/// Xác định loại giảm giá áp dụng trên hóa đơn.
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// Không áp dụng giảm giá.
    /// </summary>
    [Display(Name = "Không áp dụng giảm giá")]
    None = 0,

    /// <summary>
    /// Giảm giá theo phần trăm (%) trên tổng hóa đơn.
    /// Ví dụ: 10% sẽ giảm 10% trên tổng số tiền.
    /// </summary>
    [Display(Name = "Giảm theo phần trăm")]
    Percentage = 1,

    /// <summary>
    /// Giảm giá theo một số tiền cố định.
    /// Ví dụ: Giảm trực tiếp 50,000 VNĐ trên tổng hóa đơn.
    /// </summary>
    [Display(Name = "Giảm theo số tiền cố định")]
    Amount = 2,

    /// <summary>
    /// Giảm giá theo chương trình khuyến mãi đặc biệt.
    /// - Ví dụ: Giảm giá ngày lễ, sự kiện, flash sale.
    /// </summary>
    [Display(Name = "Giảm giá theo chương trình khuyến mãi")]
    Promotional = 3,

    /// <summary>
    /// Giảm giá theo mã giảm giá hoặc voucher.
    /// - Ví dụ: Nhập mã "DISCOUNT50" để được giảm 50,000 VNĐ.
    /// </summary>
    [Display(Name = "Giảm giá theo mã giảm giá")]
    Coupon = 4
}