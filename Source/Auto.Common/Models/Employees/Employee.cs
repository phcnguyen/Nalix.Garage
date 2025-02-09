using Auto.Common.Models.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Auto.Common.Models.Employees;

/// <summary>
/// Lớp đại diện cho nhân viên.
/// </summary>
public class Employee
{
    /// <summary>
    /// Mã nhân viên.
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Họ và tên nhân viên.
    /// </summary>
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters.")]
    public string FullName { get; set; }

    /// <summary>
    /// Ngày sinh của nhân viên.
    /// </summary>
    [Required(ErrorMessage = "Birth date is required.")]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Số điện thoại của nhân viên.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(15, ErrorMessage = "Phone number must not exceed 15 characters.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Vị trí công việc của nhân viên.
    /// </summary>
    public Position Position { get; set; } = Position.None;

    /// <summary>
    /// Danh sách công việc sửa chữa của nhân viên.
    /// </summary>
    public List<RepairTask> TaskList { get; set; } = [];

    /// <summary>
    /// Mức lương của nhân viên.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Salary cannot be negative.")]
    public decimal Salary { get; set; }

    /// <summary>
    /// Email của nhân viên.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email { get; set; } // Optional email field

    /// <summary>
    /// Danh sách lịch làm việc của nhân viên.
    /// </summary>
    public List<WorkSchedule> WorkSchedules { get; set; } = [];

    /// <summary>
    /// Thêm một lịch làm việc cho nhân viên.
    /// </summary>
    public void AddWorkSchedule(DateOnly date, bool isWorking, bool isOnLeave, LeaveType leaveType)
    {
        WorkSchedules.Add(new WorkSchedule
        {
            Date = date,
            IsWorking = isWorking,
            IsOnLeave = isOnLeave,
            LeaveType = leaveType
        });
    }

    /// <summary>
    /// Kiểm tra nhân viên có làm việc vào ngày cụ thể không.
    /// </summary>
    public bool IsWorkingOn(DateOnly date)
        => WorkSchedules.Find(ws => ws.Date == date)?.IsWorking ?? false;

    /// <summary>
    /// Đếm số ngày nghỉ của nhân viên trong một tháng.
    /// </summary>
    public int GetLeaveDaysInMonth(int month, int year) => WorkSchedules
        .Where(ws => ws.IsOnLeave && ws.Date.Month == month && ws.Date.Year == year)
        .ToList().Count;

    /// <summary>
    /// Đếm số ngày nghỉ của nhân viên trong một tháng theo loại nghỉ.
    /// </summary>
    public int GetLeaveDaysInMonth(int month, int year, LeaveType leaveType = LeaveType.None)
        => WorkSchedules
            .Where(ws =>
                ws.IsOnLeave && ws.Date.Month == month &&
                ws.Date.Year == year && (leaveType == LeaveType.None || ws.LeaveType == leaveType))
            .Count();
}