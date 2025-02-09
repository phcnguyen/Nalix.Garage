using Auto.Common.Models.Repair;
using System.Collections.Generic;

namespace Auto.Common.Models;

/// <summary>
/// Lớp đại diện cho xe.
/// </summary>
public class Car
{
    /// <summary>
    /// Mã xe.
    /// </summary>
    public int CarId { get; set; }

    /// <summary>
    /// Biển số xe.
    /// </summary>
    public string LicensePlate { get; set; }

    /// <summary>
    /// Hãng xe.
    /// </summary>
    public string CarBrand { get; set; }

    /// <summary>
    /// Model xe.
    /// </summary>
    public string CarModel { get; set; }

    /// <summary>
    /// Màu sắc.
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// Số khung.
    /// </summary>
    public string FrameNumber { get; set; }

    /// <summary>
    /// Số máy.
    /// </summary>
    public string EngineNumber { get; set; }

    /// <summary>
    /// Chủ xe.
    /// </summary>
    public Customer Owner { get; set; }

    /// <summary>
    /// Lịch sử sửa chữa của xe.
    /// </summary>
    public List<RepairHistory> RepairHistory { get; set; }
}