using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entities.Bill.Transactions;

/// <summary>
/// Trạng thái của một giao dịch tài chính.
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Giao dịch đang chờ xử lý.
    /// - Hệ thống chưa hoàn tất việc xác nhận hoặc chưa nhận được phản hồi từ cổng thanh toán.
    /// </summary>
    [Display(Name = "Đang chờ xử lý")]
    Pending = 1,

    /// <summary>
    /// Giao dịch đã được xử lý thành công.
    /// - Tiền đã được chuyển hoặc nhận đúng như yêu cầu.
    /// </summary>
    [Display(Name = "Hoàn tất")]
    Completed = 2,

    /// <summary>
    /// Giao dịch không thành công.
    /// - Có thể do lỗi hệ thống, không đủ tiền, hoặc bị từ chối bởi cổng thanh toán.
    /// </summary>
    [Display(Name = "Thất bại")]
    Failed = 3
}