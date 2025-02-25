using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Service;

/// <summary>
/// Lớp đại diện cho một dịch vụ trong hóa đơn.
/// </summary>
[Table(nameof(ServiceItem))]
public class ServiceItem
{
    /// <summary>
    /// Mã dịch vụ (Unique Identifier).
    /// </summary>
    [Key]
    public int ServiceItemId { get; set; }

    /// <summary>
    /// Mô tả của dịch vụ.
    /// </summary>
    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(255, ErrorMessage = "Description must not exceed 255 characters.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Loại dịch vụ (Sửa chữa, Bảo dưỡng,...).
    /// </summary>
    public ServiceType Type { get; set; } = ServiceType.None;

    /// <summary>
    /// Đơn giá của dịch vụ.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, 9999999.99, ErrorMessage = "Unit price must be between 0.01 and 9,999,999.99.")]
    public decimal UnitPrice { get; set; }
}