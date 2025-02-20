using System.ComponentModel;

namespace Auto.Common.Models.Vehicles;

/// <summary>
/// Enum định nghĩa các màu xe phổ biến.
/// </summary>
public enum CarColor : byte
{
    [Description("Không xác định")]
    None = 0,

    [Description("Đen")]
    Black = 1,

    [Description("Trắng")]
    White = 2,

    [Description("Xanh dương")]
    Blue = 3,

    [Description("Đỏ")]
    Red = 4,

    [Description("Bạc")]
    Silver = 5,

    [Description("Vàng")]
    Yellow = 6,

    [Description("Xám")]
    Gray = 7,

    [Description("Xanh lá")]
    Green = 8,

    [Description("Nâu")]
    Brown = 9,

    [Description("Cam")]
    Orange = 10,

    [Description("Tím")]
    Purple = 11,

    [Description("Hồng")]
    Pink = 12,

    [Description("Xanh ngọc (Cyan)")]
    Cyan = 13,

    [Description("Hồng đậm (Magenta)")]
    Magenta = 14,

    [Description("Be (Kem)")]
    Beige = 15,

    [Description("Đồng")]
    Copper = 16,

    [Description("Vàng Champagne")]
    Champagne = 17,

    [Description("khác")]
    Other = 255
}