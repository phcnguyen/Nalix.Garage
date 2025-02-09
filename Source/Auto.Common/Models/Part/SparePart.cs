using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Part;

/// <summary>
/// Lớp đại diện cho phụ tùng.
/// </summary>
public class SparePart
{
    /// <summary>
    /// Mã phụ tùng.
    /// </summary>
    public int PartId { get; set; }

    /// <summary>
    /// Tên phụ tùng.
    /// </summary>
    [StringLength(100, ErrorMessage = "Part name must not exceed 100 characters.")]
    public string PartName { get; set; } = string.Empty;

    /// <summary>
    /// Loại phụ tùng.
    /// </summary>
    [StringLength(50, ErrorMessage = "Part type must not exceed 50 characters.")]
    public string PartType { get; set; } = string.Empty;

    /// <summary>
    /// Giá bán của phụ tùng.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than 0.")]
    public decimal SellingPrice { get; set; }

    /// <summary>
    /// Số lượng tồn kho của phụ tùng.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Inventory quantity must be a positive integer.")]
    public int InventoryQuantity { get; set; }

    /// <summary>
    /// Nhà cung cấp của phụ tùng.
    /// </summary>
    public Supplier Supplier { get; set; } = null!;
}