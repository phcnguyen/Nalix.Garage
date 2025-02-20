using Auto.Common.Models.Bill.Transactions;
using Auto.Common.Models.Part;
using Auto.Common.Models.Payments;
using Auto.Common.Models.Repair;
using Auto.Common.Models.Service;
using Auto.Common.Models.Vehicles;
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
    [Key]
    public int InvoiceId { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    [Required]
    public int OwnerId { get; set; }

    /// <summary>
    /// Mã xe liên quan đến lịch sử sửa chữa.
    /// </summary>
    [Required]
    public int CarId { get; set; }

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
    /// Danh sách phụ tùng thay thế.
    /// </summary>
    public virtual List<SparePart> SpareParts { get; set; } = [];

    /// <summary>
    /// Danh sách cho đơn sửa chữa.
    /// </summary>
    public virtual List<RepairOrder> RepairOrder { get; set; } = [];

    /// <summary>
    /// Danh sách dịch vụ thực hiện.
    /// </summary>
    public virtual List<ServiceItem> ServiceItems { get; set; } = [];

    /// <summary>
    /// Danh sách các giao dịch thanh toán.
    /// </summary>
    public virtual List<Transaction> TransactionList { get; set; } = [];

    /// <summary>
    /// Tổng tiền trước thuế và giảm giá.
    /// </summary>
    public decimal Subtotal { get; set; }

    /// <summary>
    /// Số tiền thuế thực tế.
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Số tiền giảm giá thực tế.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Tổng số tiền cần thanh toán sau thuế và giảm giá.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Số tiền còn nợ.
    /// </summary>
    public decimal BalanceDue => TotalAmount - TransactionList.Sum(t => t.Amount);

    /// <summary>
    /// Số tiền khách đã thanh toán.
    /// </summary>
    public decimal AmountPaid() => Math.Max(0, TransactionList?
        .Where(t => t.Type == TransactionType.Revenue)
        .Sum(t => t.Amount) ?? 0);

    public void UpdateTotals()
    {
        Subtotal = (ServiceItems?.Sum(s => s.UnitPrice * s.Quantity) ?? 0) +
                   (SpareParts?.Sum(p => p.SellingPrice) ?? 0);

        DiscountAmount = DiscountType == DiscountType.Percentage ? Subtotal * Discount : Discount;
        TaxAmount = (Subtotal - DiscountAmount) * ((decimal)TaxRate / 100);
        TotalAmount = Subtotal - DiscountAmount + TaxAmount;
    }
}