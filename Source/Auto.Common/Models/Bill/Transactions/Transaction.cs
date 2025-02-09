using System;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Bill.Transactions;

/// <summary>
/// Lớp đại diện cho giao dịch.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Loại giao dịch (e.g., "Thu", "Chi").
    /// </summary>
    public TransactionType Type { get; set; }

    /// <summary>
    /// Số tiền giao dịch.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Transaction amount must be greater than 0.")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Mô tả giao dịch.
    /// </summary>
    [StringLength(255, ErrorMessage = "Description must not exceed 255 characters.")]
    public string Description { get; set; }

    /// <summary>
    /// Ngày giao dịch.
    /// </summary>
    public DateTime TransactionDate { get; set; } = DateTime.Now;
}