using System.ComponentModel;

namespace Auto.Common.Models.Service;

/// <summary>
/// Enum đại diện cho loại dịch vụ.
/// </summary>
public enum ServiceType : byte
{
    [Description("Không xác định")]
    None = 0,

    [Description("Bảo dưỡng định kỳ")]
    Maintenance = 1,

    [Description("Dịch vụ sửa chữa")]
    Repair = 2,

    [Description("Kiểm tra xe")]
    Inspection = 3,

    [Description("Khác")]
    Other = 255
}