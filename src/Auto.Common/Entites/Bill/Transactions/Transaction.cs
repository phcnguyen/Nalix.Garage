using Auto.Common.Models.Payments;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entites.Bill.Transactions;

/// <summary>
/// Đại diện cho một giao dịch tài chính, bao gồm các thông tin về số tiền, phương thức thanh toán và trạng thái.
/// </summary>
[Table(nameof(Transaction))]
public class Transaction
{
    /// <summary>
    /// Mã giao dịch duy nhất trong hệ thống.
    /// </summary>
    [Key]
    public int TransactionId { get; set; }

    /// <summary>
    /// Mã hóa đơn liên quan đến giao dịch.
    /// </summary>
    [ForeignKey(nameof(Invoice))]
    public int InvoiceId { get; set; }

    /// <summary>
    /// Loại giao dịch
    /// - <see cref="TransactionType.Revenue"/>: Giao dịch thu tiền
    /// - <see cref="TransactionType.Expense"/>: Giao dịch chi tiền
    /// - <see cref="TransactionType.DebtPayment"/>: Giao dịch trả nợ
    /// - <see cref="TransactionType.RepairCost"/>: Chi phí sửa chữa.
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Phương thức thanh toán của giao dịch
    /// - Ví dụ: Tiền mặt, chuyển khoản, thẻ tín dụng, ví điện tử.
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Số tiền liên quan đến giao dịch.
    /// - Giá trị phải lớn hơn 0.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Transaction amount must be greater than 0.")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Trạng thái của giao dịch.
    /// - <see cref="TransactionStatus.Pending"/>: Đang chờ xử lý
    /// - <see cref="TransactionStatus.Completed"/>: Đã hoàn thành
    /// - <see cref="TransactionStatus.Failed"/>: Thất bại.
    /// </summary>
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    /// <summary>
    /// Mô tả chi tiết về giao dịch (tùy chọn)
    /// - Không được vượt quá 255 ký tự.
    /// </summary>
    [StringLength(255, ErrorMessage = "Description must not exceed 255 characters.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ngày thực hiện giao dịch
    /// - Mặc định là thời điểm tạo giao dịch.
    /// </summary>
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Người đã tạo giao dịch trong hệ thống
    /// </summary>
    public int CreatedBy { get; set; }

    /// <summary>
    /// Người gần nhất chỉnh sửa giao dịch
    /// </summary>
    public int? ModifiedBy { get; set; }
}