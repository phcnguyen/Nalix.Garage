using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Suppliers;

/// <summary>
/// Lớp đại diện cho số điện thoại của nhà cung cấp.
/// </summary>
[Table(nameof(SupplierPhone))]
public class SupplierPhone
{
    /// <summary>
    /// Mã định danh duy nhất của số điện thoại nhà cung cấp.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Mã nhà cung cấp liên kết với số điện thoại này.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Supplier))]
    public int SupplierId { get; set; }

    /// <summary>
    /// Thông tin nhà cung cấp liên quan (Navigation Property).
    /// </summary>
    public Supplier Supplier { get; set; }

    /// <summary>
    /// Số điện thoại của nhà cung cấp (10-12 chữ số).
    /// </summary>
    [Required]
    [MaxLength(12)]
    [RegularExpression(@"^\d{10,12}$", ErrorMessage = "The phone number must be between 10-12 digits.")]
    public string PhoneNumber { get; set; } = string.Empty;
}