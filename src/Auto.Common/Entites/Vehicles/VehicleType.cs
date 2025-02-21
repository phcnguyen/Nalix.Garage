using System.ComponentModel;

namespace Auto.Common.Entites.Vehicles;

public enum VehicleType : byte
{
    [Description("Không xác định")]
    None = 0,

    [Description("Sedan - Xe du lịch")]
    Sedan = 1,

    [Description("SUV - Xe thể thao đa dụng")]
    SUV = 2,

    [Description("Hatchback - Xe cỡ nhỏ")]
    Hatchback = 3,

    [Description("Coupe - Xe thể thao")]
    Coupe = 4,

    [Description("Convertible - Xe mui trần")]
    Convertible = 5,

    [Description("Pickup - Xe bán tải")]
    Pickup = 6,

    [Description("Khác")]
    Other = 255
}