using Auto.Common.Models.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Auto.Common.Models.Employees;

/// <summary>
/// Đại diện cho một nhân viên trong hệ thống, bao gồm thông tin cá nhân,
/// vị trí công việc, mức lương, danh sách công việc sửa chữa và lịch làm việc.
/// </summary>
public class Employee
{
    /// <summary>
    /// Mã định danh duy nhất của nhân viên.
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Mã nhân viên duy nhất.
    /// </summary>
    [Required(ErrorMessage = "Employee code is required.")]
    [StringLength(20, ErrorMessage = "Employee code must not exceed 20 characters.")]
    public string EmployeeCode { get; set; }

    /// <summary>
    /// Họ và tên đầy đủ của nhân viên.
    /// </summary>
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters.")]
    public string FullName { get; set; }

    /// <summary>
    /// Địa chỉ email của nhân viên.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email { get; set; } // Optional email field

    /// <summary>
    /// Số điện thoại của nhân viên.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(15, ErrorMessage = "Phone number must not exceed 15 characters.")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Ngày sinh của nhân viên.
    /// </summary>
    [Required(ErrorMessage = "Birth date is required.")]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// NGiới tính của nhân viên.
    /// </summary>
    public Gender Gender { get; set; } = Gender.Unknown;

    /// <summary>
    /// Vị trí công việc của nhân viên.
    /// </summary>
    public Position Position { get; set; } = Position.None;

    /// <summary>
    /// Mức lương của nhân viên.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Salary cannot be negative.")]
    public decimal Salary { get; set; }

    /// <summary>
    /// Danh sách các công việc sửa chữa mà nhân viên đang phụ trách.
    /// </summary>
    public virtual List<RepairTask> TaskList { get; set; } = [];

    /// <summary>
    /// Danh sách lịch làm việc của nhân viên.
    /// </summary>
    public virtual List<WorkSchedule> WorkSchedules { get; set; } = [];

    /// <summary>
    /// Thêm một lịch làm việc cho nhân viên.
    /// </summary>
    /// <param name="date">Ngày làm việc.</param>
    /// <param name="isWorking">Xác định nhân viên có đi làm vào ngày này hay không.</param>
    /// <param name="isOnLeave">Xác định nhân viên có nghỉ phép vào ngày này hay không.</param>
    /// <param name="leaveType">Loại nghỉ phép (nếu có).</param>
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
    /// Kiểm tra xem nhân viên có làm việc vào một ngày cụ thể không.
    /// </summary>
    /// <param name="date">Ngày cần kiểm tra.</param>
    /// <returns>Trả về <c>true</c> nếu nhân viên làm việc vào ngày đó, ngược lại trả về <c>false</c>.</returns>
    public bool IsWorkingOn(DateOnly date)
        => WorkSchedules.Find(ws => ws.Date == date)?.IsWorking ?? false;

    /// <summary>
    /// Đếm số ngày nghỉ của nhân viên trong một tháng.
    /// </summary>
    /// <param name="month">Tháng cần kiểm tra.</param>
    /// <param name="year">Năm cần kiểm tra.</param>
    /// <returns>Số ngày nghỉ trong tháng.</returns>
    public int GetLeaveDaysInMonth(int month, int year) => WorkSchedules
        .Where(ws => ws.IsOnLeave && ws.Date.Month == month && ws.Date.Year == year)
        .ToList().Count;

    /// <summary>
    /// Đếm số ngày nghỉ của nhân viên trong một tháng theo loại nghỉ cụ thể.
    /// </summary>
    /// <param name="month">Tháng cần kiểm tra.</param>
    /// <param name="year">Năm cần kiểm tra.</param>
    /// <param name="leaveType">Loại nghỉ cần đếm. Nếu không truyền, sẽ tính tất cả loại nghỉ.</param>
    /// <returns>Số ngày nghỉ theo loại nghỉ chỉ định.</returns>
    public int GetLeaveDaysInMonth(int month, int year, LeaveType leaveType = LeaveType.None)
        => WorkSchedules
            .Where(ws =>
                ws.IsOnLeave && ws.Date.Month == month &&
                ws.Date.Year == year && (leaveType == LeaveType.None || ws.LeaveType == leaveType))
            .Count();
}