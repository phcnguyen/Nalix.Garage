namespace Auto.Common.Models.Bill.Transactions;

/// <summary>
/// Trạng thái của một giao dịch tài chính.
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Giao dịch đang chờ xử lý.
    /// - Hệ thống chưa hoàn tất việc xác nhận hoặc chưa nhận được phản hồi từ cổng thanh toán.
    /// </summary>
    Pending,

    /// <summary>
    /// Giao dịch đã được xử lý thành công.
    /// - Tiền đã được chuyển hoặc nhận đúng như yêu cầu.
    /// </summary>
    Completed,

    /// <summary>
    /// Giao dịch không thành công.
    /// - Có thể do lỗi hệ thống, không đủ tiền, hoặc bị từ chối bởi cổng thanh toán.
    /// </summary>
    Failed
}