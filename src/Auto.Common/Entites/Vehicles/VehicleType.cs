using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entites.Vehicles;

public enum VehicleType : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    [Display(Name = "Sedan - Xe du lịch")]
    Sedan = 1,

    [Display(Name = "SUV - Xe thể thao đa dụng")]
    SUV = 2,

    [Display(Name = "Hatchback - Xe cỡ nhỏ")]
    Hatchback = 3,

    [Display(Name = "Coupe - Xe thể thao")]
    Coupe = 4,

    [Display(Name = "Convertible - Xe mui trần")]
    Convertible = 5,

    [Display(Name = "Pickup - Xe bán tải")]
    Pickup = 6,

    [Display(Name = "Khác")]
    Other = 255
}