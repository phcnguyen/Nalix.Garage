using Auto.Common.Models.Repair;
using Auto.Common.Models.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Models.Customers;

/// <summary>
/// Lớp đại diện cho khách hàng.
/// </summary>
public class Customer
{
    /// <summary>
    /// Mã khách hàng.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerId { get; set; }

    /// <summary>
    /// Họ và tên khách hàng.
    /// </summary>
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters.")]
    public string FullName { get; set; }

    /// <summary>
    /// Số điện thoại của khách hàng.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone number must not exceed 30 characters.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Email của khách hàng.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Địa chỉ của khách hàng.
    /// </summary>
    [StringLength(255, ErrorMessage = "Address must not exceed 255 characters.")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Sinh nhật của khách hàng.
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Loại khách hàng.
    /// </summary>
    public CustomerType Type { get; set; } = CustomerType.Individual;

    /// <summary>
    /// Cấp độ thành viên.
    /// </summary>
    public MembershipLevel Membership { get; set; } = MembershipLevel.Standard;

    /// <summary>
    /// Chi tiêu của khách hàng.
    /// </summary>
    [StringLength(20, ErrorMessage = "Tax code must not exceed 20 characters.")]
    public string TaxCode { get; set; } = string.Empty;

    /// <summary>
    /// Công nợ của khách hàng.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Debt cannot be negative.")]
    public decimal Debt { get; set; } = 0;

    /// <summary>
    /// Danh sách xe của khách hàng.
    /// </summary>
    public virtual List<Car> CarList { get; set; } = [];

    /// <summary>
    /// Lịch sử sửa chữa của khách hàng.
    /// </summary>
    public virtual List<RepairHistory> RepairHistory { get; set; } = [];
}