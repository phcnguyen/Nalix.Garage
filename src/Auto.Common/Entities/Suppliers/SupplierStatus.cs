using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entities.Suppliers;

/// <summary>
/// Enum đại diện cho trạng thái của nhà cung cấp.
/// </summary>
public enum SupplierStatus : byte
{
    /// <summary>
    /// Đang hợp tác.
    /// </summary>
    [Display(Name = "Đang hợp tác")]
    Active = 1,

    /// <summary>
    /// Ngừng hợp tác.
    /// </summary>
    [Display(Name = "Ngừng hợp tác")]
    Inactive = 2,

    /// <summary>
    /// Đối tác tiềm năng.
    /// </summary>
    [Display(Name = "Đối tác tiềm năng")]
    Potential = 3,

    /// <summary>
    /// Tạm dừng hợp tác (do vi phạm điều khoản, chờ xem xét lại).
    /// </summary>
    [Display(Name = "Tạm dừng hợp tác")]
    Suspended = 4
}