using Auto.Common.Models.Repair;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Cars;

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
    /// Id chủ xe.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Biển số xe.
    /// </summary>
    [StringLength(10, ErrorMessage = "License plate must not exceed 10 characters.")]
    public string LicensePlate { get; set; }

    /// <summary>
    /// Hãng xe.
    /// </summary>
    public CarBrand CarBrand { get; set; } = CarBrand.None;

    /// <summary>
    /// Model xe.
    /// </summary>
    [StringLength(50, ErrorMessage = "Car model must not exceed 50 characters.")]
    public string CarModel { get; set; }

    /// <summary>
    /// Màu sắc.
    /// </summary>
    public CarColor Color { get; set; } = CarColor.None;

    /// <summary>
    /// Số khung.
    /// </summary>
    [StringLength(20, ErrorMessage = "Frame number must not exceed 20 characters.")]
    public string FrameNumber { get; set; }

    /// <summary>
    /// Số máy.
    /// </summary>
    [StringLength(20, ErrorMessage = "Engine number must not exceed 20 characters.")]
    public string EngineNumber { get; set; }

    /// <summary>
    /// Lịch sử sửa chữa của xe.
    /// </summary>
    public virtual List<RepairHistory> RepairHistory { get; set; }
}