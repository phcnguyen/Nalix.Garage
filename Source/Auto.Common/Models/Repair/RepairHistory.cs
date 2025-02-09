using Auto.Common.Models.Cars;
using System;
using System.Collections.Generic;

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
    /// Xe liên quan đến lịch sử sửa chữa.
    /// </summary>
    public Car Car { get; set; }

    /// <summary>
    /// Ngày sửa chữa.
    /// </summary>
    public DateTime RepairDate { get; set; }

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public List<RepairTask> RepairTaskList { get; set; }
}