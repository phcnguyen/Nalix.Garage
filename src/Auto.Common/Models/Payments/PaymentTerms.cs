using System.ComponentModel;

namespace Auto.Common.Models.Payments;

/// <summary>
/// Enum đại diện cho các điều khoản thanh toán.
/// </summary>
public enum PaymentTerms : byte
{
    /// <summary>
    /// Không xác định.
    /// - Điều khoản thanh toán chưa được xác định rõ ràng.
    /// </summary>
    [Description("Không xác định")]
    None = 0,

    /// <summary>
    /// Thanh toán ngay khi nhận hàng.
    /// - Khách hàng thanh toán ngay khi nhận hàng hóa hoặc dịch vụ.
    /// </summary>
    [Description("Thanh toán ngay khi nhận hàng")]
    DueOnReceipt = 1,

    /// <summary>
    /// Thanh toán trong 7 ngày.
    /// - Khách hàng phải thanh toán trong vòng 7 ngày sau khi nhận hàng hóa hoặc dịch vụ.
    /// </summary>
    [Description("Thanh toán trong 7 ngày")]
    Net7 = 2,

    /// <summary>
    /// Thanh toán trong 15 ngày.
    /// - Khách hàng phải thanh toán trong vòng 15 ngày sau khi nhận hàng hóa hoặc dịch vụ.
    /// </summary>
    [Description("Thanh toán trong 15 ngày")]
    Net15 = 3,

    /// <summary>
    /// Thanh toán trong 30 ngày.
    /// - Khách hàng phải thanh toán trong vòng 30 ngày sau khi nhận hàng hóa hoặc dịch vụ.
    /// </summary>
    [Description("Thanh toán trong 30 ngày")]
    Net30 = 4,

    /// <summary>
    /// Thỏa thuận riêng.
    /// - Điều khoản thanh toán sẽ được thỏa thuận riêng giữa các bên.
    /// </summary>
    [Description("Thỏa thuận riêng")]
    Custom = 255
}