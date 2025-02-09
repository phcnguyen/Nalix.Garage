using Auto.Common.Models.Bill.Transactions;
using Auto.Common.Models.Cars;
using Auto.Common.Models.Part;
using Auto.Common.Models.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Auto.Common.Models.Bill;

/// <summary>
/// Lớp đại diện cho hóa đơn gara ô tô.
/// </summary>
public class Invoice
{
    /// <summary>
    /// Mã hóa đơn.
    /// </summary>
    public int InvoiceId { get; set; }

    /// <summary>
    /// Số hóa đơn (mã duy nhất).
    /// </summary>
    [Required(ErrorMessage = "Invoice number is required.")]
    public string InvoiceNumber { get; set; }

    /// <summary>
    /// Ngày lập hóa đơn.
    /// </summary>
    public DateTime InvoiceDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Người tạo hóa đơn.
    /// </summary>
    [StringLength(50)]
    public string CreatedBy { get; set; }

    /// <summary>
    /// Người chỉnh sửa hóa đơn.
    /// </summary>
    [StringLength(50)]
    public string ModifiedBy { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Biển số xe khách hàng.
    /// </summary>
    [Required(ErrorMessage = "Car license plate is required.")]
    [StringLength(15)]
    public string CarLicensePlate { get; set; }

    /// <summary>
    /// Hãng xe.
    /// </summary>
    public CarBrand CarBrand { get; set; }

    /// <summary>
    /// Dòng xe.
    /// </summary>
    [StringLength(50)]
    public string CarModel { get; set; }

    /// <summary>
    /// Năm sản xuất.
    /// </summary>
    [Range(1900, 2100)]
    public int CarYear { get; set; }

    /// <summary>
    /// Loại giảm giá (phần trăm hoặc số tiền).
    /// </summary>
    public DiscountType DiscountType { get; set; } = DiscountType.None;

    /// <summary>
    /// Trạng thái thanh toán của hóa đơn.
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;

    /// <summary>
    /// Tổng tiền trước thuế và giảm giá.
    /// </summary>
    public decimal Subtotal =>
        (ServiceItems?.Sum(s => s.TotalPrice) ?? 0) +
        (SpareParts?.Sum(p => p.SellingPrice) ?? 0);

    /// <summary>
    /// Giá trị giảm giá.
    /// </summary>
    public decimal Discount { get; set; } = 0;

    /// <summary>
    /// Số tiền giảm giá thực tế.
    /// </summary>
    public decimal DiscountAmount =>
        DiscountType == DiscountType.Percentage ? Subtotal * Discount : Discount;

    /// <summary>
    /// Tỷ lệ thuế (5% hoặc 10%).
    /// </summary>
    [Range(0, 1)]
    public decimal TaxRate { get; set; } = 0.1m;

    /// <summary>
    /// Số tiền thuế thực tế.
    /// </summary>
    public decimal TaxAmount => (Subtotal - DiscountAmount) * TaxRate;

    /// <summary>
    /// Tổng số tiền cần thanh toán sau thuế và giảm giá.
    /// </summary>
    public decimal TotalDue => Subtotal - DiscountAmount + TaxAmount;

    /// <summary>
    /// Số tiền khách đã thanh toán.
    /// </summary>
    public decimal AmountPaid => TransactionList?
        .Where(t => t.Type == TransactionType.Revenue)
        .Sum(t => t.Amount) ?? 0;

    /// <summary>
    /// Số tiền còn nợ.
    /// </summary>
    public decimal BalanceDue => TotalDue - AmountPaid;

    /// <summary>
    /// Danh sách phụ tùng thay thế.
    /// </summary>
    public virtual List<SparePart> SpareParts { get; set; } = [];

    /// <summary>
    /// Danh sách dịch vụ thực hiện.
    /// </summary>
    public virtual List<ServiceItem> ServiceItems { get; set; } = [];

    /// <summary>
    /// Danh sách các giao dịch thanh toán.
    /// </summary>
    public virtual List<Transaction> TransactionList { get; set; } = [];
}