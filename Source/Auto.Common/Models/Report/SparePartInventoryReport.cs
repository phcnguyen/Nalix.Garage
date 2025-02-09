namespace Auto.Common.Models.Report;

/// <summary>
/// Lớp đại diện cho báo cáo tồn kho phụ tùng.
/// </summary>
public class SparePartInventoryReport
{
    /// <summary>
    /// Tên phụ tùng.
    /// </summary>
    public string PartName { get; set; }

    /// <summary>
    /// Số lượng tồn kho của phụ tùng.
    /// </summary>
    public int InventoryQuantity { get; set; }
}