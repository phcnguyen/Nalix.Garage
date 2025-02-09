using System;
using System.Collections.Generic;

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
    /// Ngày lập hóa đơn.
    /// </summary>
    public DateTime InvoiceDate { get; set; }

    /// <summary>
    /// Khách hàng liên quan đến hóa đơn.
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    /// Tổng số tiền của hóa đơn.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Trạng thái thanh toán của hóa đơn.
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; }

    /// <summary>
    /// Danh sách các giao dịch liên quan đến hóa đơn.
    /// </summary>
    public List<Transaction> TransactionList { get; set; }
}