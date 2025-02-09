namespace Auto.Common.Models.Bill;

/// <summary>
/// Trạng thái thanh toán
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Đã thanh toán
    /// </summary>
    Paid,

    /// <summary>
    /// Đang chờ xử lý
    /// </summary>
    Pending,

    /// <summary>
    /// Quá hạn
    /// </summary>
    Overdue
}