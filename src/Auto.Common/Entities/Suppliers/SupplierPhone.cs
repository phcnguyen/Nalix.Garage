using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Suppliers;

[Table(nameof(SupplierPhone))]
public class SupplierPhone
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SupplierId { get; set; }

    [Required]
    [MaxLength(12)]
    [RegularExpression(@"^\d{10,12}$", ErrorMessage = "Phone number must be 10-12 digits.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [ForeignKey(nameof(SupplierId))]
    public Supplier Supplier { get; set; }
}