using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Employees;

/// <summary>
/// Lớp đại diện cho nhân viên.
/// </summary>
public class Employee
{
    /// <summary>
    /// Mã nhân viên.
    /// </summary>
    [Key]
    public int EmployeeId { get; set; }

    /// <summary>
    /// Tên nhân viên.
    /// </summary>
    [Required(ErrorMessage = "Employee name is required.")]
    [StringLength(50)]
    public string Name { get; set; }

    /// <summary>
    /// Giới tính.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Ngày sinh.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Địa chỉ nhân viên.
    /// </summary>
    [StringLength(200)]
    public string Address { get; set; }

    /// <summary>
    /// Số điện thoại nhân viên.
    /// </summary>
    [StringLength(14)]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Email nhân viên.
    /// </summary>
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Chức vụ.
    /// </summary>
    public Position Position { get; set; }

    /// <summary>
    /// Lịch làm việc.
    /// </summary>
    public virtual List<WorkSchedule> WorkSchedules { get; set; } = [];

    /// <summary>
    /// Ngày bắt đầu làm việc.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Ngày kết thúc hợp đồng.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Trạng thái công việc.
    /// </summary>
    public EmploymentStatus Status { get; set; }

    /// <summary>
    /// Danh sách đơn nghỉ phép.
    /// </summary>
    public virtual List<LeaveType> LeaveTypes { get; set; } = [];

    /// <summary>
    /// Cập nhật trạng thái công việc.
    /// </summary>
    public void UpdateStatus()
    {
        if (EndDate.HasValue && EndDate.Value < DateTime.Now)
        {
            Status = EmploymentStatus.Inactive;
        }
    }
}