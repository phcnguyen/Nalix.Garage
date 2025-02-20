using System.ComponentModel;

namespace Auto.Common.Models.Suppliers;

/// <summary>
/// Enum đại diện cho trạng thái của nhà cung cấp.
/// </summary>
public enum SupplierStatus : byte
{
    /// <summary>
    /// Đang hợp tác.
    /// </summary>
    [Description("Đang hợp tác")]
    Active = 1,

    /// <summary>
    /// Ngừng hợp tác.
    /// </summary>
    [Description("Ngừng hợp tác")]
    Inactive = 2,

    /// <summary>
    /// Đối tác tiềm năng.
    /// </summary>
    [Description("Đối tác tiềm năng")]
    Potential = 3,

    /// <summary>
    /// Tạm dừng hợp tác (do vi phạm điều khoản, chờ xem xét lại).
    /// </summary>
    [Description("Tạm dừng hợp tác")]
    Suspended = 4
}