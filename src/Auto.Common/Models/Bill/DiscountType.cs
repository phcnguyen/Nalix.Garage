namespace Auto.Common.Models.Bill;

/// <summary>
/// Xác định loại giảm giá áp dụng trên hóa đơn.
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// Không áp dụng giảm giá.
    /// </summary>
    None,

    /// <summary>
    /// Giảm giá theo phần trăm (%) trên tổng hóa đơn.
    /// Ví dụ: 10% sẽ giảm 10% trên tổng số tiền.
    /// </summary>
    Percentage,

    /// <summary>
    /// Giảm giá theo một số tiền cố định.
    /// Ví dụ: Giảm trực tiếp 50,000 VNĐ trên tổng hóa đơn.
    /// </summary>
    Amount
}