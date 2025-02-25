using Auto.Common.Entities.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Auto.Common.Entities.Repair;

/// <summary>
/// Lớp đại diện cho lịch sử sửa chữa.
/// </summary>
[Table(nameof(RepairHistory))]
public class RepairHistory
{
    /// <summary>
    /// Mã lịch sử sửa chữa.
    /// </summary>
    [Key]
    public int HistoryId { get; set; }

    /// <summary>
    /// Mã xe liên quan đến lịch sử sửa chữa.
    /// </summary>
    [ForeignKey(nameof(Vehicles.Vehicle))]
    public int VehicleId { get; set; }

    /// <summary>
    /// Thông tin xe liên quan (Navigation Property).
    /// </summary>
    public virtual Vehicle Vehicle { get; set; }

    /// <summary>
    /// Ngày sửa chữa.
    /// </summary>
    public DateTime RepairDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public virtual ICollection<RepairTask> RepairTaskList { get; set; } = [];

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost => (RepairTaskList ?? []).Sum(task => task.ServiceItem?.UnitPrice ?? 0);
}