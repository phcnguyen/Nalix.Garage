using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entites.Service;

/// <summary>
/// Enum đại diện cho loại dịch vụ.
/// </summary>
public enum ServiceType : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    [Display(Name = "Bảo dưỡng định kỳ")]
    Maintenance = 1,

    [Display(Name = "Dịch vụ sửa chữa")]
    Repair = 2,

    [Display(Name = "Kiểm tra xe")]
    Inspection = 3,

    [Display(Name = "Khác")]
    Other = 255
}