using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Service;

/// <summary>
/// Lớp đại diện cho một dịch vụ trong hóa đơn.
/// </summary>
public class ServiceItem
{
    /// <summary>
    /// Mô tả của dịch vụ.
    /// </summary>
    [StringLength(255)]
    public string Description { get; set; }

    /// <summary>
    /// Số lượng dịch vụ.
    /// </summary>
    [Range(1, uint.MaxValue)]
    public int Quantity { get; set; }

    /// <summary>
    /// Đơn giá của dịch vụ.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Tổng giá của dịch vụ.
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice;
}