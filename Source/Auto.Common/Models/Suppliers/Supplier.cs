using Auto.Common.Models.Part;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Suppliers;

/// <summary>
/// Lớp đại diện cho nhà cung cấp.
/// </summary>
public class Supplier
{
    /// <summary>
    /// Mã nhà cung cấp (Unique identifier).
    /// </summary>
    public int SupplierId { get; set; }

    /// <summary>
    /// Tên nhà cung cấp.
    /// </summary>
    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(100, ErrorMessage = "Supplier name must not exceed 100 characters.")]
    public string Name { get; set; }

    /// <summary>
    /// Email của nhà cung cấp.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email { get; set; }

    /// <summary>
    /// Địa chỉ của nhà cung cấp.
    /// </summary>
    [StringLength(255, ErrorMessage = "Address must not exceed 255 characters.")]
    public string Address { get; set; }

    /// <summary>
    /// Số điện thoại của nhà cung cấp.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number.")]
    [StringLength(30, ErrorMessage = "Phone number must not exceed 30 characters.")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Tài khoản ngân hàng để thanh toán.
    /// </summary>
    [StringLength(50, ErrorMessage = "Bank account must not exceed 50 characters.")]
    public string BankAccount { get; set; }

    /// <summary>
    /// Mã số thuế của nhà cung cấp.
    /// </summary>
    [StringLength(20, ErrorMessage = "Tax code must not exceed 20 characters.")]
    public string TaxCode { get; set; }

    /// <summary>
    /// Điều khoản thanh toán.
    /// </summary>
    public PaymentTerms PaymentTerms { get; set; } = PaymentTerms.Unknown;

    /// <summary>
    /// Những loại phụ tùng cung cấp.
    /// </summary>
    public virtual List<SparePart> SpareParts { get; set; } = [];
}