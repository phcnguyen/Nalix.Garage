namespace Auto.Common.Models.Employees;

/// <summary>
/// Đại diện cho giới tính của nhân viên trong hệ thống.
/// Được sử dụng để phân loại và quản lý thông tin nhân sự.
/// </summary>
public enum Gender
{
    /// <summary>
    /// Giới tính nam.
    /// </summary>
    Male,

    /// <summary>
    /// Giới tính nữ.
    /// </summary>
    Female,

    /// <summary>
    /// Giới tính không xác định hoặc không được cung cấp.
    /// Giá trị mặc định nếu không có thông tin.
    /// </summary>
    Unknown
}