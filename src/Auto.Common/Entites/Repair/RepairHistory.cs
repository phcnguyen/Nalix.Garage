using Auto.Common.Entites.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Auto.Common.Entites.Repair;

/// <summary>
/// Lớp đại diện cho lịch sử sửa chữa.
/// </summary>
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
    [ForeignKey(nameof(Vehicle))]
    public int CarId { get; set; }

    /// <summary>
    /// Thông tin xe liên quan (Navigation Property).
    /// </summary>
    public virtual Vehicle Car { get; set; }

    /// <summary>
    /// Ngày sửa chữa.
    /// </summary>
    public DateTime RepairDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public virtual List<RepairTask> RepairTaskList { get; set; } = [];

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    public decimal TotalCost() => RepairTaskList?.Sum(task => task.ServiceItem?.UnitPrice ?? 0) ?? 0;
}