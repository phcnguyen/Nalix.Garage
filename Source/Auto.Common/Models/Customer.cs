using Auto.Common.Models.Repair;
using System.Collections.Generic;

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
    public string FullName { get; set; }

    /// <summary>
    /// Số điện thoại của khách hàng.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Địa chỉ của khách hàng.
    /// </summary>
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
    public decimal Debt { get; set; }
}