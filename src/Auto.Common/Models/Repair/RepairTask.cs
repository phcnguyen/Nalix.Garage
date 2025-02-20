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
    [Key]
    public int RepairTaskId { get; set; }

    /// <summary>
    /// Nhân viên thực hiện công việc sửa chữa.
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Mô tả công việc sửa chữa.
    /// </summary>
    [StringLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Đơn giá của công việc sửa chữa.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; } = 100_000;

    /// <summary>
    /// Ngày bắt đầu công việc.
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Ngày hoàn thành công việc sửa chữa (nếu đã xong).
    /// </summary>
    public DateTime? CompletionDate { get; set; }

    /// <summary>
    /// Thời gian ước tính để hoàn thành công việc (tính bằng giờ).
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Duration must be positive.")]
    public double EstimatedDuration { get; set; } = TimeSpan.FromHours(1).TotalHours;

    /// <summary>
    /// Trạng thái công việc sửa chữa.
    /// </summary>
    public RepairOrderStatus Status { get; set; } = RepairOrderStatus.Pending;
}