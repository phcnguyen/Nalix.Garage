namespace Auto.Common.Models.Employees;

/// <summary>
/// Đại diện cho các loại nghỉ phép của nhân viên trong hệ thống.
/// Được sử dụng để quản lý và theo dõi lịch nghỉ làm.
/// </summary>
public enum LeaveType
{
    /// <summary>
    /// Không có loại nghỉ phép cụ thể.
    /// Sử dụng khi nhân viên không có đơn nghỉ hoặc chưa xác định loại nghỉ phép.
    /// </summary>
    None,

    /// <summary>
    /// Nghỉ phép có lương.
    /// Nhân viên vẫn được trả lương trong thời gian nghỉ.
    /// </summary>
    PaidLeave,

    /// <summary>
    /// Nghỉ phép không lương.
    /// Nhân viên không được trả lương trong thời gian nghỉ.
    /// </summary>
    UnpaidLeave,

    /// <summary>
    /// Nghỉ bệnh.
    /// Được sử dụng khi nhân viên nghỉ vì lý do sức khỏe.
    /// </summary>
    SickLeave,

    /// <summary>
    /// Nghỉ phép thai sản.
    /// Áp dụng cho nhân viên nghỉ để sinh con hoặc chăm sóc con nhỏ.
    /// </summary>
    MaternityLeave,

    /// <summary>
    /// Nghỉ phép lễ hội.
    /// Sử dụng cho các ngày nghỉ lễ chính thức theo quy định.
    /// </summary>
    HolidayLeave
}