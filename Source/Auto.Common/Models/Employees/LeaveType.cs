namespace Auto.Common.Models.Employees;

/// <summary>
/// Enum đại diện cho các loại nghỉ phép.
/// </summary>
public enum LeaveType
{
    /// <summary>
    /// Nghỉ phép không xác định.
    /// </summary>
    None,

    /// <summary>
    /// Nghỉ phép có lương.
    /// </summary>
    PaidLeave,

    /// <summary>
    /// Nghỉ phép không lương.
    /// </summary>
    UnpaidLeave,

    /// <summary>
    /// Nghỉ bệnh.
    /// </summary>
    SickLeave,

    /// <summary>
    /// Nghỉ phép thai sản.
    /// </summary>
    MaternityLeave,

    /// <summary>
    /// Nghỉ phép lễ hội.
    /// </summary>
    HolidayLeave
}