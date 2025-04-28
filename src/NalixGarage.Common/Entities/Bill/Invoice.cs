using NalixGarage.Common.Entities.Customers;
using NalixGarage.Common.Entities.Employees;
using NalixGarage.Common.Entities.Part;
using NalixGarage.Common.Entities.Repair;
using NalixGarage.Common.Entities.Transactions;
using NalixGarage.Common.Enums.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace NalixGarage.Common.Entities.Bill;

/// <summary>
/// Lớp đại diện cho hóa đơn gara ô tô.
/// </summary>
[Table(nameof(Invoice))]
public class Invoice
{
    #region Fields

    private decimal _discount;
    private string _invoiceNumber = string.Empty;

    #endregion

    #region Identification Properties

    /// <summary>
    /// Mã hóa đơn.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    [ForeignKey(nameof(Customers.Customer))]
    public int OwnerId { get; set; }

    /// <summary>
    /// Thông tin chủ xe (Navigation Property).
    /// </summary>
    public virtual Customer Customer { get; set; }

    /// <summary>
    /// Số hóa đơn (mã duy nhất).
    /// </summary>
    [Required(ErrorMessage = "Invoice number is required.")]
    [MaxLength(30, ErrorMessage = "Invoice number must not exceed 30 characters.")]
    public string InvoiceNumber
    {
        get => _invoiceNumber;
        set => _invoiceNumber = value?.Trim() ?? string.Empty;
    }

    #endregion

    #region Audit Properties

    /// <summary>
    /// Người tạo hóa đơn.
    /// </summary>
    [ForeignKey(nameof(Employee))]
    public int CreatedById { get; set; }

    /// <summary>
    /// Người chỉnh sửa hóa đơn.
    /// </summary>
    [ForeignKey(nameof(Employee))]
    public int? ModifiedById { get; set; }

    /// <summary>
    /// Ngày lập hóa đơn.
    /// </summary>
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    #endregion

    #region Payment Details Properties

    /// <summary>
    /// Trạng thái thanh toán của hóa đơn.
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    /// <summary>
    /// Tỷ lệ thuế (5% hoặc 10%).
    /// </summary>
    public TaxRateType TaxRate { get; set; } = TaxRateType.VAT10;

    /// <summary>
    /// Loại giảm giá (phần trăm hoặc số tiền).
    /// </summary>
    public DiscountType DiscountType { get; set; } = DiscountType.None;

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

    #endregion

    #region Related Entities Properties

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

    #endregion

    #region Calculated Properties

    /// <summary>
    /// Tính tổng tiền trước thuế và giảm giá.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Subtotal { get; private set; }

    /// <summary>
    /// Số tiền giảm giá thực tế.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Số tiền thuế thực tế.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TaxAmount { get; private set; }

    /// <summary>
    /// Tổng số tiền cần thanh toán sau thuế và giảm giá.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Số tiền còn nợ.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal BalanceDue { get; private set; }

    /// <summary>
    /// Hóa đơn đã thanh toán đủ chưa?
    /// </summary>
    public bool IsFullyPaid => BalanceDue <= 0;

    #endregion

    #region Methods

    /// <summary>
    /// Số tiền khách đã thanh toán.
    /// </summary>
    public decimal AmountPaid()
    {
        return TransactionList?.Where(t => t.Type == TransactionType.Revenue).Sum(t => t.Amount) ?? 0;
    }

    /// <summary>
    /// Tính toán lại các giá trị tài chính của hóa đơn.
    /// </summary>
    public void Recalculate()
    {
        Subtotal = (SpareParts?.Sum(p => p.SellingPrice) ?? 0) + (RepairOrders?.Sum(o => o.TotalRepairCost) ?? 0);
        DiscountAmount = DiscountType == DiscountType.Percentage ? Subtotal * Discount / 100 : Discount;
        TaxAmount = (Subtotal - DiscountAmount) * ((decimal)TaxRate / 100);
        TotalAmount = Subtotal - DiscountAmount + TaxAmount;
        BalanceDue = TotalAmount - AmountPaid();
    }

    #endregion
}