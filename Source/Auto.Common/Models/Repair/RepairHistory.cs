using System;
using System.Collections.Generic;
using System.Linq;

namespace Auto.Common.Models.Repair;

/// <summary>
/// Lớp đại diện cho lịch sử sửa chữa.
/// </summary>
public class RepairHistory
{
    /// <summary>
    /// Mã lịch sử sửa chữa.
    /// </summary>
    public int HistoryId { get; set; }

    /// <summary>
    /// Mã xe liên quan đến lịch sử sửa chữa.
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// Ngày sửa chữa.
    /// </summary>
    public DateTime RepairDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public List<RepairTask> RepairTaskList { get; set; } = [];

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    public decimal TotalCost() => RepairTaskList.Sum(task => task.UnitPrice);
}