using Auto.Common.Entities.Part;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Repair;

/// <summary>
/// Bảng trung gian giữa RepairOrder và SparePart.
/// </summary>
[Table(nameof(RepairOrderSparePart))]
public class RepairOrderSparePart
{
    #region Fields

    // Hiện tại không có private fields, để lại region này cho tính nhất quán.

    #endregion

    #region Foreign Key Properties

    /// <summary>
    /// Khóa ngoại tới RepairOrder.
    /// </summary>
    [ForeignKey(nameof(RepairOrder))]
    public int RepairOrderId { get; set; }

    /// <summary>
    /// Thông tin đơn sửa chữa liên quan (Navigation Property).
    /// </summary>
    public RepairOrder RepairOrder { get; set; } = null!;

    /// <summary>
    /// Khóa ngoại tới SparePart.
    /// </summary>
    [ForeignKey(nameof(SparePart))]
    public int SparePartId { get; set; }

    /// <summary>
    /// Thông tin phụ tùng liên quan (Navigation Property).
    /// </summary>
    public SparePart SparePart { get; set; } = null!;

    #endregion

    #region Additional Properties

    /// <summary>
    /// Số lượng phụ tùng sử dụng trong đơn sửa chữa.
    /// </summary>
    public int Quantity { get; set; }

    #endregion
}