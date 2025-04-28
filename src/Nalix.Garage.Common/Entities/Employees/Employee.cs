using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nalix.Garage.Common.Entities.Employees;

/// <summary>
/// Lớp đại diện cho nhân viên.
/// </summary>
[Table(nameof(Employee))]
public class Employee
{
    #region Fields

    private string _name;
    private string _email;
    private string _address;
    private string _phoneNumber;

    private DateTime? _dateOfBirth;
    private DateTime? _endDate;
    private DateTime _startDate = DateTime.UtcNow;

    #endregion

    #region Identification Properties

    /// <summary>
    /// Mã nhân viên.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Tên nhân viên.
    /// </summary>
    [Required(ErrorMessage = "Employee name is required.")]
    [MaxLength(50)]
    public string Name
    {
        get => _name;
        set => _name = value?.Trim() ?? string.Empty;
    }

    #endregion

    #region Personal Information Properties

    /// <summary>
    /// Giới tính.
    /// </summary>
    public Gender Gender { get; set; } = Gender.None;

    /// <summary>
    /// Ngày sinh.
    /// </summary>
    public DateTime? DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (value.HasValue && value > DateTime.UtcNow)
                throw new ArgumentException("Date of birth cannot be in the future.");
            _dateOfBirth = value;
        }
    }

    #endregion

    #region Contact Information Properties

    /// <summary>
    /// Địa chỉ nhân viên.
    /// </summary>
    [MaxLength(200)]
    public string Address
    {
        get => _address;
        set => _address = value?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Số điện thoại nhân viên.
    /// </summary>
    [MaxLength(14)]
    [RegularExpression(@"^\d{10,14}$", ErrorMessage = "Phone number must be 10-14 digits.")]
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => _phoneNumber = value?.Trim() ?? string.Empty;
    }

    /// <summary>
    /// Email nhân viên.
    /// </summary>
    [MaxLength(50)]
    [EmailAddress]
    public string Email
    {
        get => _email;
        set => _email = value?.Trim() ?? string.Empty;
    }

    #endregion

    #region Employment Details Properties

    /// <summary>
    /// Chức vụ.
    /// </summary>
    public Position Position { get; set; } = Position.None;

    /// <summary>
    /// Ngày bắt đầu làm việc.
    /// </summary>
    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (value > (EndDate ?? DateTime.MaxValue))
                throw new ArgumentException("Start date cannot be later than end date.");
            _startDate = value;
            UpdateStatus();
        }
    }

    /// <summary>
    /// Ngày kết thúc hợp đồng.
    /// </summary>
    public DateTime? EndDate
    {
        get => _endDate;
        set
        {
            _endDate = value;
            UpdateStatus();
        }
    }

    /// <summary>
    /// Trạng thái công việc.
    /// </summary>
    public EmploymentStatus Status { get; set; } = EmploymentStatus.None;

    #endregion

    #region Methods

    /// <summary>
    /// Cập nhật trạng thái công việc.
    /// </summary>
    public void UpdateStatus()
    {
        if (EndDate.HasValue && EndDate.Value < DateTime.UtcNow)
        {
            Status = EmploymentStatus.Inactive;
        }
        else if (StartDate > DateTime.UtcNow)
        {
            Status = EmploymentStatus.Pending;
        }
        else
        {
            Status = EmploymentStatus.Active;
        }
    }

    #endregion
}