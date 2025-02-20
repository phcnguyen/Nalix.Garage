using System;

namespace Auto.Common.Models.Employees;

/// <summary>
/// Đại diện cho lịch làm việc của nhân viên.
/// </summary>
/// <param name="date">Ngày làm việc hoặc nghỉ phép.</param>
/// <param name="isWorking">Trạng thái làm việc.</param>
/// <param name="isOnLeave">Trạng thái nghỉ phép.</param>
/// <param name="leaveType">Loại nghỉ phép (mặc định là <see cref="LeaveType.None"/>).</param>
public class WorkSchedule(DateOnly date, bool isWorking, bool isOnLeave, LeaveType leaveType = LeaveType.None)
{
    public WorkSchedule() : this(default, default, default, LeaveType.None)
    {
    }

    /// <summary>
    /// Ngày làm việc hoặc nghỉ phép.
    /// </summary>
    public DateOnly Date { get; set; } = date;

    /// <summary>
    /// Xác định nhân viên có làm việc trong ngày này không.
    /// - `true`: Nhân viên đi làm.
    /// - `false`: Nhân viên không làm việc (có thể do nghỉ phép hoặc ngày nghỉ theo quy định).
    /// </summary>
    public bool IsWorking { get; set; } = isWorking;

    /// <summary>
    /// Xác định nhân viên có nghỉ phép trong ngày này không.
    /// - `true`: Nhân viên đang nghỉ phép.
    /// - `false`: Không phải ngày nghỉ phép (có thể là ngày làm việc hoặc ngày nghỉ chung).
    /// </summary>
    public bool IsOnLeave { get; set; } = isOnLeave;

    /// <summary>
    /// Loại nghỉ phép của nhân viên (nếu có).
    /// Mặc định là <see cref="LeaveType.None"/> nếu không phải ngày nghỉ phép.
    /// </summary>
    public LeaveType LeaveType { get; set; } = leaveType;

    /// <summary>
    /// Hiển thị thông tin lịch làm việc dưới dạng chuỗi.
    /// </summary>
    public override string ToString()
        => $"{Date}: {(IsWorking ? "Working" : "Off")} {(IsOnLeave ? $"- {LeaveType}" : "")}";
}