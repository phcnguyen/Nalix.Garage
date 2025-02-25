using Auto.Common.Entities.Part;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Repair;

/// <summary>
/// Bảng trung gian giữa RepairOrder và SparePart.
/// </summary>
[Table(nameof(RepairOrderSparePart))]
public class RepairOrderSparePart
{
    /// <summary>
    /// Khóa ngoại tới RepairOrder.
    /// </summary>
    [ForeignKey(nameof(RepairOrder))]
    public int RepairOrderId { get; set; }

    public RepairOrder RepairOrder { get; set; } = null!;

    /// <summary>
    /// Khóa ngoại tới SparePart.
    /// </summary>
    [ForeignKey(nameof(SparePart))]
    public int SparePartId { get; set; }

    public SparePart SparePart { get; set; } = null!;

    /// <summary>
    /// Số lượng phụ tùng sử dụng trong đơn sửa chữa.
    /// </summary>
    public int Quantity { get; set; }
}