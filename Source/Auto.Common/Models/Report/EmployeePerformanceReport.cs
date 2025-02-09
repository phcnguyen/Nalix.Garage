using Auto.Common.Models.Employees;

namespace Auto.Common.Models.Report;

/// <summary>
/// Lớp đại diện cho báo cáo hiệu suất nhân viên.
/// </summary>
public class EmployeePerformanceReport
{
    /// <summary>
    /// Nhân viên liên quan đến báo cáo.
    /// </summary>
    public Employee Employee { get; set; }

    /// <summary>
    /// Số lượng công việc đã hoàn thành.
    /// </summary>
    public int CompletedTaskCount { get; set; }
}