using Auto.Common.Models.Cars;
using Auto.Common.Models.Repair;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models;

/// <summary>
/// Lớp đại diện cho khách hàng.
/// </summary>
public class Customer
{
    /// <summary>
    /// Mã khách hàng.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Họ và tên khách hàng.
    /// </summary>
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, ErrorMessage = "Full name must not exceed 100 characters.")]
    public string FullName { get; set; }

    /// <summary>
    /// Email của khách hàng.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email { get; set; } // Optional email field

    /// <summary>
    /// Số điện thoại của khách hàng.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(30, ErrorMessage = "Phone number must not exceed 30 characters.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Địa chỉ của khách hàng.
    /// </summary>
    [StringLength(255, ErrorMessage = "Address must not exceed 255 characters.")]
    public string Address { get; set; }

    /// <summary>
    /// Danh sách xe của khách hàng.
    /// </summary>
    public List<Car> CarList { get; set; }

    /// <summary>
    /// Lịch sử sửa chữa của khách hàng.
    /// </summary>
    public List<RepairHistory> RepairHistory { get; set; }

    /// <summary>
    /// Công nợ của khách hàng.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Debt cannot be negative.")]
    public decimal Debt { get; set; }
}