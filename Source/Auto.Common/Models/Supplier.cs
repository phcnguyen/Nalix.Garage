using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models;

/// <summary>
/// Lớp đại diện cho nhà cung cấp.
/// </summary>
public class Supplier
{
    /// <summary>
    /// Mã nhà cung cấp (Unique identifier).
    /// </summary>
    public int SupplierId { get; set; }

    /// <summary>
    /// Tên nhà cung cấp.
    /// </summary>
    [Required(ErrorMessage = "Supplier name is required.")]
    [StringLength(100, ErrorMessage = "Supplier name must not exceed 100 characters.")]
    public string Name { get; set; }

    /// <summary>
    /// Địa chỉ của nhà cung cấp.
    /// </summary>
    [StringLength(255, ErrorMessage = "Address must not exceed 255 characters.")]
    public string Address { get; set; }

    /// <summary>
    /// Số điện thoại của nhà cung cấp.
    /// </summary>
    [Phone(ErrorMessage = "Invalid phone number.")]
    [StringLength(30, ErrorMessage = "Phone number must not exceed 30 characters.")]
    public string PhoneNumber { get; set; }
}