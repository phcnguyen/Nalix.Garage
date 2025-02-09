using System;

namespace Auto.Common.Models.Report;

/// <summary>
/// Lớp đại diện cho báo cáo doanh thu.
/// </summary>
public class RevenueReport
{
    /// <summary>
    /// Thời gian báo cáo.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Doanh thu.
    /// </summary>
    public decimal Revenue { get; set; }
}