using Auto.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Enums.Cars;

/// <summary>
/// Enum định nghĩa các màu xe phổ biến và đặc biệt.
/// </summary>
public enum CarColor : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    // Màu cơ bản
    [HexColor("#000000")]
    [Display(Name = "Đen")]
    Black = 1,

    [HexColor("#FFFFFF")]
    [Display(Name = "Trắng")]
    White = 2,

    [HexColor("#808080")]
    [Display(Name = "Xám")]
    Gray = 3,

    [HexColor("#C0C0C0")]
    [Display(Name = "Bạc")]
    Silver = 4,

    // Màu phổ biến
    [HexColor("#FF0000")]
    [Display(Name = "Đỏ")]
    Red = 5,

    [HexColor("#0000FF")]
    [Display(Name = "Xanh dương")]
    Blue = 6,

    [HexColor("#008000")]
    [Display(Name = "Xanh lá")]
    Green = 7,

    [HexColor("#FFFF00")]
    [Display(Name = "Vàng")]
    Yellow = 8,

    [HexColor("#A52A2A")]
    [Display(Name = "Nâu")]
    Brown = 9,

    [HexColor("#FFA500")]
    [Display(Name = "Cam")]
    Orange = 10,

    [HexColor("#800080")]
    [Display(Name = "Tím")]
    Purple = 11,

    [HexColor("#FFC0CB")]
    [Display(Name = "Hồng")]
    Pink = 12,

    [HexColor("#00FFFF")]
    [Display(Name = "Xanh ngọc (Cyan)")]
    Cyan = 13,

    [HexColor("#F7E7CE")]
    [Display(Name = "Vàng Champagne")]
    Champagne = 14,

    // Màu ánh kim (Metallic)
    [HexColor("#2F4F4F")]
    [Display(Name = "Graphite (Xám đậm)")]
    Graphite = 15,

    [HexColor("#191970")]
    [Display(Name = "Midnight Blue (Xanh đêm)")]
    MidnightBlue = 16,

    [HexColor("#B76E79")]
    [Display(Name = "Rose Gold (Vàng hồng)")]
    RoseGold = 17,

    [HexColor("#FDFEFE")]
    [Display(Name = "Pearl White (Trắng ngọc trai)")]
    PearlWhite = 18,

    [HexColor("#8C7853")]
    [Display(Name = "Bronze (Đồng)")]
    Bronze = 19,

    [HexColor("#E5E4E2")]
    [Display(Name = "Platinum (Bạch kim)")]
    Platinum = 20,

    // Màu mờ (Matte)
    [HexColor("#696969")]
    [Display(Name = "Matte Gray (Xám xi măng)")]
    MatteGray = 21,

    [HexColor("#141414")]
    [Display(Name = "Matte Black (Đen mờ)")]
    MatteBlack = 22,

    [HexColor("#003366")]
    [Display(Name = "Matte Blue (Xanh dương mờ)")]
    MatteBlue = 23,

    [HexColor("#2F6D33")]
    [Display(Name = "Matte Green (Xanh lá mờ)")]
    MatteGreen = 24,

    // Màu đặc biệt
    [HexColor("#FF4500")]
    [Display(Name = "Lava Red (Đỏ dung nham)")]
    LavaRed = 25,

    [HexColor("#556B2F")]
    [Display(Name = "Olive Green (Xanh ô liu)")]
    OliveGreen = 26,

    [HexColor("#0047AB")]
    [Display(Name = "Cobalt Blue (Xanh cobalt)")]
    CobaltBlue = 27,

    [HexColor("#70543E")]
    [Display(Name = "Mocha Brown (Nâu cà phê)")]
    MochaBrown = 28,

    [HexColor("#D4AF37")]
    [Display(Name = "Gold (Vàng ánh kim)")]
    Gold = 29,

    [HexColor("#FFB6C1")]
    [Display(Name = "Light Pink (Hồng nhạt)")]
    LightPink = 30,

    [HexColor("#4682B4")]
    [Display(Name = "Steel Blue (Xanh thép)")]
    SteelBlue = 31,

    [HexColor("#9400D3")]
    [Display(Name = "Dark Violet (Tím than)")]
    DarkViolet = 32,

    [HexColor("#E9967A")]
    [Display(Name = "Salmon (Hồng cá hồi)")]
    Salmon = 33,

    [HexColor("#32CD32")]
    [Display(Name = "Lime Green (Xanh chanh)")]
    LimeGreen = 34,

    [HexColor("#DC143C")]
    [Display(Name = "Crimson Red (Đỏ thẫm)")]
    CrimsonRed = 35,

    [HexColor("#008B8B")]
    [Display(Name = "Dark Cyan (Xanh lục lam đậm)")]
    DarkCyan = 36,

    [HexColor("#FFD700")]
    [Display(Name = "Amber (Hổ phách)")]
    Amber = 37,

    [HexColor("#CD7F32")]
    [Display(Name = "Copper (Đồng đỏ)")]
    Copper = 38,

    [HexColor("#BDB76B")]
    [Display(Name = "Khaki (Vàng cát)")]
    Khaki = 39,

    [Display(Name = "Khác")]
    Other = 255
}
