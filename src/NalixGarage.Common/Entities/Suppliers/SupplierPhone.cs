using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NalixGarage.Common.Entities.Suppliers;

/// <summary>
/// Lớp đại diện cho số điện thoại của nhà cung cấp.
/// </summary>
[Table(nameof(SupplierPhone))]
public class SupplierPhone
{
    #region Fields

    // Hiện tại không có private fields, để lại region này cho tính nhất quán.

    #endregion

    #region Identification Properties

    /// <summary>
    /// Mã định danh duy nhất của số điện thoại nhà cung cấp.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

    #endregion

    #region Contact Properties

    /// <summary>
    /// Số điện thoại của nhà cung cấp (10-12 chữ số).
    /// </summary>
    [Required]
    [MaxLength(12)]
    [RegularExpression(@"^\d{10,12}$", ErrorMessage = "The phone number must be between 10-12 digits.")]
    public string PhoneNumber { get; set; } = string.Empty;

    #endregion
}