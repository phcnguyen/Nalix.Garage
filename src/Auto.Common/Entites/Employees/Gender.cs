using System.ComponentModel;

namespace Auto.Common.Entites.Employees;

/// <summary>
/// Đại diện cho giới tính của nhân viên.
/// </summary>
public enum Gender : byte
{
    /// <summary>
    /// Giới tính không xác định hoặc không cung cấp.
    /// </summary>
    [Description("Không xác định")]
    None = 0,

    /// <summary>
    /// Giới tính nam.
    /// </summary>
    [Description("Nam")]
    Male = 1,

    /// <summary>
    /// Giới tính nữ.
    /// </summary>
    [Description("Nữ")]
    Female = 2,

    /// <summary>
    /// Giới tính khác.
    /// </summary>
    [Description("Khác")]
    Other = 255
}