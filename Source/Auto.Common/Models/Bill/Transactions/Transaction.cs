using System;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Bill.Transactions;

/// <summary>
/// Lớp đại diện cho giao dịch.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Mã giao dịch duy nhất.
    /// </summary>
    public int TransactionId { get; set; }

    /// <summary>
    /// Mã hóa đơn liên quan (nếu có).
    /// </summary>
    public int? InvoiceId { get; set; }

    /// <summary>
    /// Loại giao dịch (Thu hoặc Chi).
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Phương thức thanh toán.
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Số tiền giao dịch.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Transaction amount must be greater than 0.")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Trạng thái giao dịch.
    /// </summary>
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;

    /// <summary>
    /// Mô tả giao dịch.
    /// </summary>
    [StringLength(255, ErrorMessage = "Description must not exceed 255 characters.")]
    public string Description { get; set; }

    /// <summary>
    /// Ngày giao dịch.
    /// </summary>
    public DateTime TransactionDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Người tạo giao dịch.
    /// </summary>
    [StringLength(50)]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Người chỉnh sửa giao dịch.
    /// </summary>
    [StringLength(50)]
    public string ModifiedBy { get; set; }
}