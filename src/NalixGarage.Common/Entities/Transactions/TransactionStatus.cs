﻿using System.ComponentModel.DataAnnotations;

namespace NalixGarage.Common.Entities.Transactions;

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
    Failed = 3,

    /// <summary>
    /// Giao dịch đã bị hủy bởi khách hàng hoặc hệ thống trước khi xử lý xong.
    /// </summary>
    [Display(Name = "Đã hủy")]
    Canceled = 4,

    /// <summary>
    /// Giao dịch đã được hoàn tiền cho khách hàng.
    /// - Áp dụng khi có lỗi hoặc khách hàng yêu cầu hoàn tiền.
    /// </summary>
    [Display(Name = "Đã hoàn tiền")]
    Refunded = 5,

    /// <summary>
    /// Giao dịch bị tạm giữ để kiểm tra thêm.
    /// - Có thể do nghi ngờ gian lận hoặc cần xác minh thêm thông tin.
    /// </summary>
    [Display(Name = "Đang xem xét")]
    UnderReview = 6
}
