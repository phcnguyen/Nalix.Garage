using Auto.Common.Models.Bill.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Auto.Common.Models.Bill;

/// <summary>
/// Lớp đại diện cho hóa đơn.
/// </summary>
public class Invoice
{
    /// <summary>
    /// Mã hóa đơn.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Người tạo hóa đơn.
    /// </summary>
    [StringLength(50, ErrorMessage = "Created by must not exceed 50 characters.")]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Người chỉnh sửa hóa đơn.
    /// </summary>
    [StringLength(50, ErrorMessage = "Modified by must not exceed 50 characters.")]
    public string ModifiedBy { get; set; }

    /// <summary>
    /// Ngày lập hóa đơn.
    /// </summary>
    public DateTime InvoiceDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Khách hàng liên quan đến hóa đơn.
    /// </summary>
    [Required(ErrorMessage = "Customer is required.")]
    public Customer Customer { get; set; }

    /// <summary>
    /// Tổng số tiền của hóa đơn.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Total amount must be a positive value.")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Mức giảm giá của hóa đơn.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Discount must be a positive value.")]
    public decimal Discount { get; set; } = 0; // Default to 0 if no discount

    /// <summary>
    /// Loại giảm giá (phần trăm hay tiền mặt).
    /// </summary>
    public DiscountType DiscountType { get; set; } = DiscountType.None;

    /// <summary>
    /// Tỷ lệ thuế của hóa đơn (dưới dạng phần trăm).
    /// </summary>
    [Range(0, 1, ErrorMessage = "Tax rate must be between 0 and 1.")]
    public decimal TaxRate { get; set; } = 0; // Default to 0 if no tax

    /// <summary>
    /// Trạng thái thanh toán của hóa đơn.
    /// </summary>
    [Required(ErrorMessage = "Payment status is required.")]
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Danh sách các giao dịch liên quan đến hóa đơn.
    /// </summary>
    public List<Transaction> TransactionList { get; set; }

    public decimal GetOutstandingBalance()
        => TotalAmount - (TransactionList?.Sum(t => t.Amount) ?? 0);
}