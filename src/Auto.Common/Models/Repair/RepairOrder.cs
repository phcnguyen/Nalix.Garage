using Auto.Common.Models.Part;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auto.Common.Models.Repair;

/// <summary>
/// Lớp đại diện cho đơn sửa chữa.
/// </summary>
public class RepairOrder
{
    /// <summary>
    /// Mã đơn sửa chữa.
    /// </summary>
    public int RepairOrderId { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Mã xe liên quan đến đơn sửa chữa.
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// Ngày lập đơn.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public List<RepairTask> RepairTaskList { get; set; } = [];

    /// <summary>
    /// Danh sách phụ tùng thay thế liên quan.
    /// </summary>
    public List<ReplacementPart> ReplacementPartList { get; set; } = [];

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    public decimal TotalRepairCost() =>
        (RepairTaskList?.Sum(task => task.UnitPrice) ?? 0) +
        (ReplacementPartList?.Sum(part => part.UnitPrice) ?? 0);
}