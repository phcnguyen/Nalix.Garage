using Auto.Common.Entites.Bill.Transactions;
using Auto.Common.Entites.Customers;
using Auto.Common.Entites.Employees;
using Auto.Common.Entites.Part;
using Auto.Common.Entites.Payments;
using Auto.Common.Entites.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Auto.Common.Entities.Bill;

/// <summary>
/// Lớp đại diện cho hóa đơn gara ô tô.
/// </summary>
[Table(nameof(Invoice))]
public class Invoice
{
    private string _invoiceNumber = string.Empty;

    /// <summary>
    /// Mã hóa đơn.
    /// </summary>
    [Key]
    public int InvoiceId { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    [ForeignKey(nameof(Customer))]
    public int OwnerId { get; set; }

    /// <summary>
    /// Thông tin chủ xe (Navigation Property).
    /// </summary>
    public virtual Customer Owner { get; set; }

    /// <summary>
    /// Người tạo hóa đơn.
    /// </summary>
    [ForeignKey(nameof(Employee))]
    public int CreatedBy { get; set; }

    /// <summary>
    /// Người chỉnh sửa hóa đơn.
    /// </summary>
    [ForeignKey(nameof(Employee))]
    public int? ModifiedBy { get; set; }

    /// <summary>
    /// Số hóa đơn (mã duy nhất).
    /// </summary>
    [Required(ErrorMessage = "Invoices number is required.")]
    [StringLength(30, ErrorMessage = "Invoices number must not exceed 30 characters.")]
    public string InvoiceNumber
    {
        get => _invoiceNumber;
        set => _invoiceNumber = value.Trim();
    }

    /// <summary>
    /// Ngày lập hóa đơn.
    /// </summary>
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Tỷ lệ thuế (5% hoặc 10%).
    /// </summary>
    public TaxRateType TaxRate { get; set; } = TaxRateType.VAT10;

    /// <summary>
    /// Loại giảm giá (phần trăm hoặc số tiền).
    /// </summary>
    public DiscountType DiscountType { get; set; } = DiscountType.None;

    /// <summary>
    /// Trạng thái thanh toán của hóa đơn.
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    /// <summary>
    /// Giá trị giảm giá.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Discount must be a positive value.")]
    public decimal Discount { get; set; } = 0;

    /// <summary>
    /// Tổng tiền trước thuế và giảm giá.
    /// </summary>
    public decimal Subtotal { get; private set; }

    /// <summary>
    /// Số tiền thuế thực tế.
    /// </summary>
    public decimal TaxAmount { get; private set; }

    /// <summary>
    /// Số tiền giảm giá thực tế.
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Tổng số tiền cần thanh toán sau thuế và giảm giá.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Số tiền còn nợ.
    /// </summary>
    public decimal BalanceDue => TotalAmount - (TransactionList?.Sum(t => t.Amount) ?? 0);

    /// <summary>
    /// Danh sách phụ tùng thay thế.
    /// </summary>
    public virtual ICollection<SparePart> SpareParts { get; set; } = [];

    /// <summary>
    /// Danh sách cho đơn sửa chữa.
    /// </summary>
    public virtual ICollection<RepairOrder> RepairOrders { get; set; } = [];

    /// <summary>
    /// Danh sách các giao dịch thanh toán.
    /// </summary>
    public virtual ICollection<Transaction> TransactionList { get; set; } = [];

    /// <summary>
    /// Số tiền khách đã thanh toán.
    /// </summary>
    public decimal AmountPaid() =>
        TransactionList?
            .Where(t => t.Type == TransactionType.Revenue)
            .Sum(t => t.Amount) ?? 0;

    /// <summary>
    /// Cập nhật tổng tiền, thuế và giảm giá.
    /// </summary>
    public void UpdateTotals()
    {
        Subtotal = (SpareParts?.Sum(p => p.SellingPrice) ?? 0) +
                   (RepairOrders?.Sum(o => o.TotalRepairCost()) ?? 0);

        DiscountAmount = DiscountType == DiscountType.Percentage
            ? Subtotal * Discount / 100
            : Discount;

        TaxAmount = (Subtotal - DiscountAmount) * ((decimal)TaxRate / 100);
        TotalAmount = Subtotal - DiscountAmount + TaxAmount;
    }
}