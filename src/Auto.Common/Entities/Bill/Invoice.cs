using Auto.Common.Entities.Bill.Transactions;
using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Employees;
using Auto.Common.Entities.Part;
using Auto.Common.Entities.Repair;
using Auto.Common.Models.Payments;
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
    private decimal _discount;
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
    [Required(ErrorMessage = "Invoice number is required.")]
    [MaxLength(30, ErrorMessage = "Invoice number must not exceed 30 characters.")]
    public string InvoiceNumber { get => _invoiceNumber; set => _invoiceNumber = value?.Trim() ?? string.Empty; }

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
    public decimal Discount
    {
        get => _discount;
        set
        {
            if (DiscountType == DiscountType.Percentage && (value < 0 || value > 100))
                throw new ArgumentException("Discount percentage must be between 0 and 100.");
            _discount = value;
        }
    }

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
    /// Tính tổng tiền trước thuế và giảm giá.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal => CalculateSubtotal();

    /// <summary>
    /// Số tiền giảm giá thực tế.
    /// </summary>
    public decimal DiscountAmount => CalculateDiscount();

    /// <summary>
    /// Số tiền thuế thực tế.
    /// </summary>
    public decimal TaxAmount => CalculateTax();

    /// <summary>
    /// Tổng số tiền cần thanh toán sau thuế và giảm giá.
    /// </summary>
    public decimal TotalAmount => CalculateTotalAmount();

    /// <summary>
    /// Số tiền còn nợ.
    /// </summary>
    public decimal BalanceDue => TotalAmount - AmountPaid();

    /// <summary>
    /// Hóa đơn đã thanh toán đủ chưa?
    /// </summary>
    public bool IsFullyPaid => BalanceDue <= 0;

    /// <summary>
    /// Số tiền khách đã thanh toán.
    /// </summary>
    public decimal AmountPaid()
        => TransactionList?.Where(t => t.Type == TransactionType.Revenue).Sum(t => t.Amount) ?? 0;

    private decimal CalculateSubtotal()
        => (SpareParts?.Sum(p => p.SellingPrice) ?? 0) + (RepairOrders?.Sum(o => o.TotalRepairCost()) ?? 0);

    private decimal CalculateDiscount()
        => DiscountType == DiscountType.Percentage ? Subtotal * Discount / 100 : Discount;

    private decimal CalculateTax() => (Subtotal - DiscountAmount) * ((decimal)TaxRate / 100);

    private decimal CalculateTotalAmount() => Subtotal - DiscountAmount + TaxAmount;
}