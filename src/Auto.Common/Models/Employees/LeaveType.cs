using System.ComponentModel;

namespace Auto.Common.Models.Employees;

/// <summary>
/// Đại diện cho các loại nghỉ phép của nhân viên.
/// </summary>
public enum LeaveType : byte
{
    /// <summary>
    /// Không có loại nghỉ phép cụ thể.
    /// </summary>
    [Description("Không xác định")]
    None = 0,

    /// <summary>
    /// Nghỉ phép có lương.
    /// </summary>
    [Description("Nghỉ phép có lương")]
    PaidLeave = 1,

    /// <summary>
    /// Nghỉ phép không lương.
    /// </summary>
    [Description("Nghỉ phép không lương")]
    UnpaidLeave = 2,

    /// <summary>
    /// Nghỉ bệnh.
    /// </summary>
    [Description("Nghỉ ốm")]
    SickLeave = 3,

    /// <summary>
    /// Nghỉ thai sản.
    /// </summary>
    [Description("Nghỉ thai sản")]
    MaternityLeave = 4,

    /// <summary>
    /// Nghỉ lễ.
    /// </summary>
    [Description("Nghỉ lễ")]
    HolidayLeave = 5,

    /// <summary>
    /// Nghỉ phép cá nhân (cưới hỏi, gia đình, v.v.).
    /// </summary>
    [Description("Nghỉ phép cá nhân")]
    PersonalLeave = 6,

    /// <summary>
    /// Nghỉ tang.
    /// </summary>
    [Description("Nghỉ tang")]
    BereavementLeave = 7,

    /// <summary>
    /// Nghỉ để học tập/nâng cao trình độ.
    /// </summary>
    [Description("Nghỉ phép học tập")]
    StudyLeave = 8
}