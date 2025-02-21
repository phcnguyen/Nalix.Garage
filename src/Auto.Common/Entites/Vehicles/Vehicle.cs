using Auto.Common.Entites.Customers;
using Auto.Common.Entites.Repair;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entites.Vehicles;

/// <summary>
/// Lớp đại diện cho xe.
/// </summary>
[Table(nameof(Vehicle))]
public class Vehicle
{
    private string _carLicensePlate = string.Empty;
    private string _engineNumber = string.Empty;
    private string _frameNumber = string.Empty;
    private string _carModel = string.Empty;

    /// <summary>
    /// Mã xe.
    /// </summary>
    [Key]
    public int CarId { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    [Required]
    [ForeignKey(nameof(Customer))]
    public int OwnerId { get; set; }

    /// <summary>
    /// Thông tin chủ xe (Navigation Property).
    /// </summary>
    public virtual Customer Owner { get; set; }

    /// <summary>
    /// Năm sản xuất.
    /// </summary>
    [Range(1900, 2100)]
    public int CarYear { get; set; } = 1900;

    /// <summary>
    /// Loại xe (Sedan, SUV, Hatchback, ...).
    /// </summary>
    public VehicleType CarType { get; set; } = VehicleType.Sedan;

    /// <summary>
    /// Màu sắc.
    /// </summary>
    public VehicleColor CarColor { get; set; } = VehicleColor.None;

    /// <summary>
    /// Hãng xe.
    /// </summary>
    public VehicleBrand CarBrand { get; set; } = VehicleBrand.None;

    /// <summary>
    /// Biển số xe khách hàng.
    /// </summary>
    [Required(ErrorMessage = "Vehicle license plate is required.")]
    [StringLength(9)]
    public string CarLicensePlate
    {
        get => _carLicensePlate;
        set => _carLicensePlate = value.Trim().ToUpper();
    }

    /// <summary>
    /// Model xe.
    /// </summary>
    [StringLength(50, ErrorMessage = "Vehicle model must not exceed 50 characters.")]
    public string CarModel
    {
        get => _carModel;
        set => _carModel = value.Trim();
    }

    /// <summary>
    /// Số khung.
    /// </summary>
    [StringLength(17, ErrorMessage = "Frame number must not exceed 17 characters.")]
    public string FrameNumber
    {
        get => _frameNumber;
        set => _frameNumber = value.Trim();
    }

    /// <summary>
    /// Số máy.
    /// </summary>
    [StringLength(17, ErrorMessage = "Engine number must not exceed 17 characters.")]
    public string EngineNumber
    {
        get => _engineNumber;
        set => _engineNumber = value.Trim();
    }

    /// <summary>
    /// Lịch sử sửa chữa của xe.
    /// </summary>
    [InverseProperty(nameof(Repair.RepairHistory))]
    public virtual ICollection<RepairHistory> RepairHistory { get; set; } = [];

    /// <summary>
    /// Ngày đăng ký xe.
    /// </summary>
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Quá trình lái xe (Km đã đi).
    /// </summary>
    [Range(0, 1_000_000, ErrorMessage = "Mileage must be a positive value.")]
    public double Mileage { get; set; } = 0;

    /// <summary>
    /// Ngày hết hạn bảo hiểm.
    /// </summary>
    public DateTime? InsuranceExpiryDate { get; set; }
}