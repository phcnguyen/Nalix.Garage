using System;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Employees;

/// <summary>
/// Lớp đại diện cho lịch làm việc của nhân viên.
/// </summary>
public class WorkSchedule
{
    /// <summary>
    /// Mã lịch làm việc.
    /// </summary>
    [Key]
    public int WorkScheduleId { get; set; }

    /// <summary>
    /// Mã nhân viên.
    /// </summary>
    [Required]
    public int EmployeeId { get; set; }

    /// <summary>
    /// Ngày làm việc.
    /// </summary>
    [Required]
    public DateTime WorkDate { get; set; }

    /// <summary>
    /// Giờ bắt đầu.
    /// </summary>
    [Required]
    public TimeSpan StartTime { get; set; }

    /// <summary>
    /// Giờ kết thúc.
    /// </summary>
    [Required]
    public TimeSpan EndTime { get; set; }

    /// <summary>
    /// Trạng thái làm việc (ví dụ: Đang làm việc, Nghỉ phép).
    /// </summary>
    [StringLength(50)]
    public string Status { get; set; }

    /// <summary>
    /// Ghi chú.
    /// </summary>
    [StringLength(200)]
    public string Notes { get; set; }

    /// <summary>
    /// Tính tổng số giờ làm việc.
    /// </summary>
    public double TotalHours => (EndTime - StartTime).TotalHours;

    /// <summary>
    /// Cập nhật trạng thái lịch làm việc.
    /// </summary>
    public void UpdateStatus(string newStatus)
    {
        Status = newStatus;
    }
}