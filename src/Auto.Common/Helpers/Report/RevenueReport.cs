using System;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Helpers.Report;

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
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal Revenue { get; set; }
}