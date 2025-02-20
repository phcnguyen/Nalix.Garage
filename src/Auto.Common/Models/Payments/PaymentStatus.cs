namespace Auto.Common.Models.Payments;

/// <summary>
/// Xác định trạng thái thanh toán của hóa đơn.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Hóa đơn chưa được thanh toán.
    /// - Khách hàng chưa thực hiện thanh toán.
    /// - Hóa đơn vẫn còn hiệu lực.
    /// </summary>
    Unpaid,

    /// <summary>
    /// Hóa đơn đã được thanh toán đầy đủ.
    /// - Không còn số dư cần thanh toán.
    /// - Không cần thực hiện thêm hành động nào.
    /// </summary>
    Paid,

    /// <summary>
    /// Thanh toán đang được xử lý.
    /// - Đã có yêu cầu thanh toán, nhưng chưa xác nhận hoàn tất.
    /// - Có thể đang chờ ngân hàng hoặc hệ thống xử lý.
    /// </summary>
    Pending,

    /// <summary>
    /// Hóa đơn đã quá hạn thanh toán.
    /// - Khách hàng chưa thanh toán sau thời hạn quy định.
    /// - Có thể áp dụng phí trễ hạn hoặc nhắc nhở thanh toán.
    /// </summary>
    Overdue,

    /// <summary>
    /// Hóa đơn bị hủy bỏ.
    /// - Không cần thanh toán nữa.
    /// - Có thể do khách hàng từ chối hoặc có lỗi trong hóa đơn.
    /// </summary>
    Canceled
}