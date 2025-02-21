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
    public int ServiceId { get; set; }

    /// <summary>
    /// Mô tả của dịch vụ.
    /// </summary>
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(255 * 2, ErrorMessage = "Description must not exceed 255 characters.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Loại dịch vụ (Sửa chữa, Bảo dưỡng,...).
    /// </summary>
    public ServiceType Type { get; set; } = ServiceType.None;

    /// <summary>
    /// Đơn giá của dịch vụ.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }
}