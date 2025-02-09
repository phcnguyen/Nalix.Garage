using Auto.Common.Models.Repair;
using System;
using System.Collections.Generic;

namespace Auto.Common.Models;

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
    public string FullName { get; set; }

    /// <summary>
    /// Ngày sinh của nhân viên.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Số điện thoại của nhân viên.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Vị trí công việc của nhân viên.
    /// </summary>
    public string Position { get; set; }

    /// <summary>
    /// Danh sách công việc sửa chữa của nhân viên.
    /// </summary>
    public List<RepairTask> TaskList { get; set; }

    /// <summary>
    /// Mức lương của nhân viên.
    /// </summary>
    public decimal Salary { get; set; }
}