using Auto.Common.Entities.Employees;
using Auto.Common.Entities.Service;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Repair;

/// <summary>
/// Lớp đại diện cho công việc sửa chữa.
/// </summary>
[Table(nameof(RepairTask))]
public class RepairTask
{
    private DateTime? _startDate;
    private DateTime? _completionDate;

    /// <summary>
    /// Mã công việc sửa chữa.
    /// </summary>
    [Key]
    public int RepairTaskId { get; set; }

    /// <summary>
    /// Nhân viên thực hiện công việc sửa chữa.
    /// </summary>
    [ForeignKey(nameof(Employee))]
    public int EmployeeId { get; set; }

    /// <summary>
    /// Thông tin nhân viên thực hiện (Navigation Property).
    /// </summary>
    public virtual Employee Employee { get; set; }

    /// <summary>
    /// Các dịch vụ sử dụng.
    /// </summary>
    [ForeignKey(nameof(ServiceItem))]
    public int ServiceItemId { get; set; }

    /// <summary>
    /// Thông tin dịch vụ liên quan (Navigation Property).
    /// </summary>
    public virtual ServiceItem ServiceItem { get; set; }

    /// <summary>
    /// Trạng thái công việc sửa chữa.
    /// </summary>
    public RepairOrderStatus Status { get; set; } = RepairOrderStatus.Pending;

    /// <summary>
    /// Ngày bắt đầu công việc.
    /// </summary>
    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            if (value.HasValue && value > DateTime.UtcNow)
                throw new ArgumentException("Start date cannot be in the future.");
            _startDate = value;
        }
    }

    /// <summary>
    /// Thời gian ước tính để hoàn thành công việc (tính bằng giờ).
    /// </summary>
    [Range(0, 1000, ErrorMessage = "Estimated duration must be between 0 and 1000 hours.")]
    public double EstimatedDuration { get; set; } = 1.0;

    /// <summary>
    /// Ngày hoàn thành công việc sửa chữa (nếu đã xong).
    /// </summary>
    public DateTime? CompletionDate
    {
        get => _completionDate;
        set
        {
            if (value.HasValue && StartDate.HasValue && value < StartDate)
                throw new ArgumentException("Completion date cannot be earlier than start date.");
            _completionDate = value;
        }
    }
}