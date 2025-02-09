using System;

namespace Auto.Common.Models.Report;

/// <summary>
/// Lớp đại diện cho báo cáo sửa chữa xe.
/// </summary>
public class CarRepairReport
{
    /// <summary>
    /// Thời gian báo cáo.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Số lượng xe đã sửa chữa.
    /// </summary>
    public int CarCount { get; set; }
}