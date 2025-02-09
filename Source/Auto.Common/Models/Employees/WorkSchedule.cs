using System;

namespace Auto.Common.Models.Employees;

public class WorkSchedule
{
    /// <summary>
    /// Ngày làm việc.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Trạng thái làm việc: true nếu là ngày làm việc, false nếu là ngày nghỉ.
    /// </summary>
    public bool IsWorking { get; set; }

    /// <summary>
    /// Trạng thái nghỉ phép: true nếu là ngày nghỉ, false nếu không phải.
    /// </summary>
    public bool IsOnLeave { get; set; }

    /// <summary>
    /// Loại nghỉ phép của nhân viên.
    /// </summary>
    public LeaveType LeaveType { get; set; } = LeaveType.None;
}