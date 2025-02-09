using Auto.Common.Models.Employees;
using System;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Repair;

/// <summary>
/// Lớp đại diện cho công việc sửa chữa.
/// </summary>
public class RepairTask
{
    /// <summary>
    /// Mã công việc sửa chữa.
    /// </summary>
    public int RepairTaskId { get; set; }

    /// <summary>
    /// Mô tả công việc sửa chữa.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string Description { get; set; }

    /// <summary>
    /// Đơn giá của công việc sửa chữa.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Nhân viên thực hiện công việc sửa chữa.
    /// </summary>
    public Employee PerformingEmployee { get; set; }

    /// <summary>
    /// Thời gian hoàn thành công việc sửa chữa (tùy chọn).
    /// </summary>
    public DateTime? CompletionDate { get; set; }
}