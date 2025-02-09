namespace Auto.Common.Models;

/// <summary>
/// Lớp đại diện cho nhà cung cấp.
/// </summary>
public class Supplier
{
    /// <summary>
    /// Tên nhà cung cấp.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Địa chỉ của nhà cung cấp.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// Số điện thoại của nhà cung cấp.
    /// </summary>
    public string PhoneNumber { get; set; }
}