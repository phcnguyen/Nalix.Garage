using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Report;

/// <summary>
/// Lớp đại diện cho báo cáo hiệu suất nhân viên.
/// </summary>
public class EmployeePerformanceReport
{
    /// <summary>
    /// Id nhân viên liên quan đến báo cáo.
    /// </summary>
    public int EmployeeId { get; set; }

    /// <summary>
    /// Tên nhân viên liên quan đến báo cáo.
    /// </summary>
    [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters.")]
    public string EmployeeName { get; set; }

    /// <summary>
    /// Số lượng công việc đã hoàn thành.
    /// </summary>
    public int CompletedTaskCount { get; set; }
}