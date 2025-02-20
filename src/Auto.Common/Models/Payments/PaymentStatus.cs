using System.ComponentModel;

namespace Auto.Common.Models.Payments;

/// <summary>
/// Xác định trạng thái thanh toán của hóa đơn.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Hóa đơn chưa được thanh toán.
    /// </summary>
    [Description("Chưa thanh toán")]
    Unpaid = 0,

    /// <summary>
    /// Hóa đơn đã được thanh toán đầy đủ.
    /// </summary>
    [Description("Đã thanh toán")]
    Paid = 1,

    /// <summary>
    /// Thanh toán đang được xử lý.
    /// </summary>
    [Description("Đang xử lý")]
    Pending = 2,

    /// <summary>
    /// Hóa đơn đã quá hạn thanh toán.
    /// </summary>
    [Description("Quá hạn")]
    Overdue = 3,

    /// <summary>
    /// Hóa đơn bị hủy bỏ.
    /// </summary>
    [Description("Đã hủy")]
    Canceled = 4,

    /// <summary>
    /// Hóa đơn đã được thanh toán một phần.
    /// </summary>
    [Description("Thanh toán một phần")]
    PartiallyPaid = 5,

    /// <summary>
    /// Hóa đơn đã được hoàn tiền cho khách hàng.
    /// </summary>
    [Description("Đã hoàn tiền")]
    Refunded = 6
}