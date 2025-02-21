using Auto.Common.Entities.Repair;
using Auto.Common.Entities.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Customers;

/// <summary>
/// Lớp đại diện cho khách hàng.
/// </summary>
[Table(nameof(Customer))]
public class Customer
{
    private string _email = string.Empty;
    private string _address = string.Empty;
    private string _taxCode = string.Empty;
    private string _fullName = string.Empty;
    private string _phoneNumber = string.Empty;

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
    public string Name
    {
        get => _fullName;
        set => _fullName = value.Trim();
    }

    /// <summary>
    /// Số điện thoại của khách hàng.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required.")]
    [StringLength(12, ErrorMessage = "Phone number must not exceed 30 characters.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value.Trim();
    }

    /// <summary>
    /// Email của khách hàng.
    /// </summary>
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(100, ErrorMessage = "Email must not exceed 100 characters.")]
    public string Email
    {
        get => _email;
        set => _email = value.Trim();
    }

    /// <summary>
    /// Địa chỉ của khách hàng.
    /// </summary>
    [StringLength(255, ErrorMessage = "Address must not exceed 255 characters.")]
    public string Address
    {
        get => _address;
        set => _address = value.Trim();
    }

    /// <summary>
    /// Mã số thuế của khách hàng (nếu có).
    /// </summary>
    [StringLength(13, ErrorMessage = "Tax code must not exceed 20 characters.")]
    public string TaxCode
    {
        get => _taxCode;
        set => _taxCode = value.Trim();
    }

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
    /// Công nợ của khách hàng.
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "Debt cannot be negative.")]
    public decimal Debt { get; set; } = 0;

    /// <summary>
    /// Danh sách xe của khách hàng.
    /// </summary>
    public virtual ICollection<Vehicle> CarList { get; set; } = [];

    /// <summary>
    /// Lịch sử sửa chữa của khách hàng.
    /// </summary>
    public virtual ICollection<RepairHistory> RepairHistory { get; set; } = [];
}