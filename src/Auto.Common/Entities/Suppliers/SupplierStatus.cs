using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entities.Suppliers;

/// <summary>
/// Enum đại diện cho trạng thái của nhà cung cấp.
/// </summary>
public enum SupplierStatus : byte
{
    /// <summary>
    /// Chưa xác định trạng thái.
    /// </summary>
    [Display(Name = "Không xác định")]
    None = 0,

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
    Suspended = 4,

    /// <summary>
    /// Nhà cung cấp mới, đang trong quá trình xem xét hợp tác.
    /// </summary>
    [Display(Name = "Đang xem xét hợp tác")]
    UnderReview = 5,

    /// <summary>
    /// Đã ký hợp đồng nhưng chưa bắt đầu cung cấp sản phẩm/dịch vụ.
    /// </summary>
    [Display(Name = "Đã ký hợp đồng, chờ kích hoạt")]
    ContractSigned = 6,

    /// <summary>
    /// Đã bị loại khỏi danh sách hợp tác vĩnh viễn.
    /// </summary>
    [Display(Name = "Bị loại khỏi hệ thống")]
    Blacklisted = 7
}
