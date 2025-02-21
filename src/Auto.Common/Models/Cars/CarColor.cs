using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Cars;

/// <summary>
/// Enum định nghĩa các màu xe phổ biến.
/// </summary>
public enum CarColor : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    [Display(Name = "Đen")]
    Black = 1,

    [Display(Name = "Trắng")]
    White = 2,

    [Display(Name = "Xanh dương")]
    Blue = 3,

    [Display(Name = "Đỏ")]
    Red = 4,

    [Display(Name = "Bạc")]
    Silver = 5,

    [Display(Name = "Vàng")]
    Yellow = 6,

    [Display(Name = "Xám")]
    Gray = 7,

    [Display(Name = "Xanh lá")]
    Green = 8,

    [Display(Name = "Nâu")]
    Brown = 9,

    [Display(Name = "Cam")]
    Orange = 10,

    [Display(Name = "Tím")]
    Purple = 11,

    [Display(Name = "Hồng")]
    Pink = 12,

    [Display(Name = "Xanh ngọc (Cyan)")]
    Cyan = 13,

    [Display(Name = "Hồng đậm (Magenta)")]
    Magenta = 14,

    [Display(Name = "Be (Kem)")]
    Beige = 15,

    [Display(Name = "Đồng")]
    Copper = 16,

    [Display(Name = "Vàng Champagne")]
    Champagne = 17,

    [Display(Name = "Khác")]
    Other = 255
}