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
    public string PartName { get; set; }

    /// <summary>
    /// Loại phụ tùng.
    /// </summary>
    public string PartType { get; set; }

    /// <summary>
    /// Giá bán của phụ tùng.
    /// </summary>
    public decimal SellingPrice { get; set; }

    /// <summary>
    /// Số lượng tồn kho của phụ tùng.
    /// </summary>
    public int InventoryQuantity { get; set; }

    /// <summary>
    /// Nhà cung cấp của phụ tùng.
    /// </summary>
    public Supplier Supplier { get; set; }
}