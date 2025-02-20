using Auto.Common.Models.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Vehicles;

/// <summary>
/// Lớp đại diện cho xe.
/// </summary>
public class Car
{
    /// <summary>
    /// Mã xe.
    /// </summary>
    [Key]
    public int CarId { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    [Key]
    public int OwnerId { get; set; }

    /// <summary>
    /// Biển số xe.
    /// </summary>
    [StringLength(10, ErrorMessage = "License plate must not exceed 10 characters.")]
    public string LicensePlate { get; set; } = "72G-00000";

    /// <summary>
    /// Hãng xe.
    /// </summary>
    public CarBrand CarBrand { get; set; } = CarBrand.None;

    /// <summary>
    /// Model xe.
    /// </summary>
    [StringLength(50, ErrorMessage = "Car model must not exceed 50 characters.")]
    public string CarModel { get; set; } = "Unknown";

    /// <summary>
    /// Màu sắc.
    /// </summary>
    public CarColor Color { get; set; } = CarColor.None;

    /// <summary>
    /// Số khung.
    /// </summary>
    [StringLength(20, ErrorMessage = "Frame number must not exceed 20 characters.")]
    public string FrameNumber { get; set; } = "Unknown";

    /// <summary>
    /// Số máy.
    /// </summary>
    [StringLength(20, ErrorMessage = "Engine number must not exceed 20 characters.")]
    public string EngineNumber { get; set; } = "Unknown";

    /// <summary>
    /// Lịch sử sửa chữa của xe.
    /// </summary>
    public virtual List<RepairHistory> RepairHistory { get; set; } = [];

    /// <summary>
    /// Loại xe (Sedan, SUV, Hatchback, ...).
    /// </summary>
    public CarType CarType { get; set; } = CarType.Sedan;

    /// <summary>
    /// Ngày đăng ký xe.
    /// </summary>
    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Quá trình lái xe (Km đã đi).
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Mileage must be a positive value.")]
    public double Mileage { get; set; } = 0;

    /// <summary>
    /// Ngày hết hạn bảo hiểm.
    /// </summary>
    public DateTime? InsuranceExpiryDate { get; set; }
}