using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Service;

/// <summary>
/// Lớp đại diện cho một dịch vụ trong hóa đơn.
/// </summary>
public class ServiceItem
{
    /// <summary>
    /// Mã dịch vụ (Unique Identifier).
    /// </summary>
    public int ServiceId { get; set; }

    /// <summary>
    /// Mô tả của dịch vụ.
    /// </summary>
    [Required(ErrorMessage = "Description is required.")]
    [StringLength(255, ErrorMessage = "Description must not exceed 255 characters.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Loại dịch vụ (Sửa chữa, Bảo dưỡng,...).
    /// </summary>
    public ServiceType Type { get; set; } = ServiceType.Other;

    /// <summary>
    /// Số lượng dịch vụ.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }

    /// <summary>
    /// Đơn giá của dịch vụ.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Tính tổng giá của dịch vụ.
    /// </summary>
    public decimal GetTotalPrice() => Quantity * UnitPrice;
}