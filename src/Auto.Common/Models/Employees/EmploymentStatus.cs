using System.ComponentModel;

namespace Auto.Common.Models.Employees;

/// <summary>
/// Trạng thái làm việc của nhân viên.
/// </summary>
public enum EmploymentStatus
{
    /// <summary>
    /// Nhân viên đang làm việc.
    /// </summary>
    [Description("Đang làm việc")]
    Active = 1,

    /// <summary>
    /// Nhân viên đã nghỉ việc.
    /// </summary>
    [Description("Đã nghỉ việc")]
    Inactive = 2,

    /// <summary>
    /// Nhân viên đang nghỉ phép.
    /// </summary>
    [Description("Đang nghỉ phép")]
    OnLeave = 3,

    /// <summary>
    /// Nhân viên bị chấm dứt hợp đồng.
    /// </summary>
    [Description("Bị sa thải")]
    Terminated = 4,

    /// <summary>
    /// Nhân viên đã nghỉ hưu.
    /// </summary>
    [Description("Đã nghỉ hưu")]
    Retired = 5
}