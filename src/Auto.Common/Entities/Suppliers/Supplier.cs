using Auto.Common.Entities.Part;
using Auto.Common.Models.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Suppliers;

/// <summary>
/// Lớp đại diện cho nhà cung cấp.
/// </summary>
[Table(nameof(Supplier))]
public class Supplier
{
    /// <summary>
    /// Mã nhà cung cấp (Unique identifier).
    /// </summary>
    [Key]
    public int SupplierId { get; set; }

    /// <summary>
    /// Tên nhà cung cấp.
    /// </summary>
    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(100, ErrorMessage = "Supplier name must not exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Email của nhà cung cấp.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ của nhà cung cấp.
    /// </summary>
    [StringLength(255, ErrorMessage = "Address must not exceed 255 characters.")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Danh sách số điện thoại của nhà cung cấp.
    /// </summary>
    public List<string> PhoneNumbers { get; set; } = [];

    /// <summary>
    /// Ghi chú về nhà cung cấp.
    /// </summary>
    public string Notes { get; set; } = string.Empty;

    /// <summary>
    /// Ngày kết thúc hợp tác (nếu có).
    /// </summary>
    public DateTime? ContractEndDate { get; set; }

    /// <summary>
    /// Ngày bắt đầu hợp tác với nhà cung cấp.
    /// </summary>
    public DateTime ContractStartDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Tài khoản ngân hàng để thanh toán.
    /// </summary>
    [StringLength(20, ErrorMessage = "Bank account must not exceed 50 characters.")]
    public string BankAccount { get; set; } = string.Empty;

    /// <summary>
    /// Mã số thuế của nhà cung cấp.
    /// </summary>
    [StringLength(13, ErrorMessage = "Tax code must not exceed 20 characters.")]
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// Trạng thái của nhà cung cấp (Hoạt động, Ngừng hợp tác,...).
    /// </summary>
    public SupplierStatus Status { get; set; } = SupplierStatus.Active;

    /// <summary>
    /// Điều khoản thanh toán.
    /// </summary>
    public PaymentTerms PaymentTerms { get; set; } = PaymentTerms.None;

    /// <summary>
    /// Những loại phụ tùng cung cấp.
    /// </summary>
    public virtual List<SparePart> SpareParts { get; set; } = [];
}