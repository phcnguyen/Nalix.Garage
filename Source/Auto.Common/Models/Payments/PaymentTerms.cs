namespace Auto.Common.Models.Payments;

/// <summary>
/// Enum đại diện cho các điều khoản thanh toán.
/// </summary>
public enum PaymentTerms
{
    /// <summary>
    /// Không xác định.
    /// </summary>
    Unknown,

    /// <summary>
    /// Thanh toán ngay khi nhận hàng.
    /// </summary>
    DueOnReceipt,

    /// <summary>
    /// Thanh toán trong 7 ngày.
    /// </summary>
    Net7,

    /// <summary>
    /// Thanh toán trong 15 ngày.
    /// </summary>
    Net15,

    /// <summary>
    /// Thanh toán trong 30 ngày.
    /// </summary>
    Net30,

    /// <summary>
    /// Thỏa thuận riêng.
    /// </summary>
    Custom
}