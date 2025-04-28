using System.ComponentModel.DataAnnotations;

namespace NalixGarage.Common.Enums.Cars;

public enum CarType : byte
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

    [Display(Name = "Minivan - Xe gia đình")]
    Minivan = 7,

    [Display(Name = "Truck - Xe tải")]
    Truck = 8,

    [Display(Name = "Bus - Xe buýt")]
    Bus = 9,

    [Display(Name = "Motorcycle - Xe máy")]
    Motorcycle = 10,

    [Display(Name = "Other - Loại khác")]
    Other = 255
}
