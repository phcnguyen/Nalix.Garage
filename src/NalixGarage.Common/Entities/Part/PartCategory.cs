using System.ComponentModel.DataAnnotations;

namespace NalixGarage.Common.Entities.Part;

/// <summary>
/// Danh mục phụ tùng ô tô.
/// </summary>
public enum PartCategory : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    // 🔥 Động cơ & truyền động
    [Display(Name = "Phụ tùng động cơ")]
    Engine = 1,

    [Display(Name = "Phụ tùng truyền động")]
    Transmission = 2,

    [Display(Name = "Hệ thống phun nhiên liệu")]
    FuelInjection = 3,

    [Display(Name = "Bộ tăng áp")]
    Turbocharger = 4,

    [Display(Name = "Hệ thống bôi trơn")]
    Lubrication = 5,

    [Display(Name = "Hệ thống làm mát")]
    Cooling = 6,

    [Display(Name = "Hệ thống nhiên liệu")]
    Fuel = 7,

    [Display(Name = "Hệ thống xả")]
    Exhaust = 8,

    [Display(Name = "Hệ thống đánh lửa")]
    Ignition = 9,

    // ⚡ Hệ thống điện & điều khiển
    [Display(Name = "Phụ tùng điện")]
    Electrical = 10,

    [Display(Name = "Cảm biến và mô-đun điều khiển")]
    SensorsAndModules = 11,

    [Display(Name = "Hệ thống chống bó cứng phanh")]
    ABS = 12,

    [Display(Name = "Hệ thống ổn định điện tử")]
    ESC = 13,

    [Display(Name = "Hệ thống chiếu sáng")]
    Lighting = 14,

    // 🚗 Hệ thống an toàn
    [Display(Name = "Phụ tùng phanh")]
    Brake = 15,

    [Display(Name = "Hệ thống an toàn")]
    Safety = 16,

    [Display(Name = "Túi khí và thiết bị an toàn")]
    Airbags = 17,

    [Display(Name = "Hệ thống khóa và an ninh")]
    SecurityAndLocking = 18,

    // 🔧 Khung gầm & treo
    [Display(Name = "Hệ thống treo")]
    Suspension = 19,

    [Display(Name = "Hệ thống lái")]
    Steering = 20,

    [Display(Name = "Bánh xe và lốp")]
    WheelAndTire = 21,

    // 🏠 Nội thất & tiện nghi
    [Display(Name = "Hệ thống điều hòa")]
    AirConditioning = 22,

    [Display(Name = "Nội thất xe")]
    Interior = 23,

    [Display(Name = "Hệ thống giải trí")]
    Entertainment = 24,

    [Display(Name = "Hệ thống định vị")]
    Navigation = 25,

    [Display(Name = "Hệ thống sưởi ghế")]
    SeatHeating = 26,

    [Display(Name = "Hệ thống làm mát ghế")]
    SeatCooling = 27,

    // 🎭 Ngoại thất & phụ kiện
    [Display(Name = "Phụ tùng thân xe")]
    Body = 28,

    [Display(Name = "Gương và kính")]
    MirrorsAndGlass = 29,

    [Display(Name = "Phụ kiện ngoại thất")]
    ExteriorAccessories = 30,

    [Display(Name = "Phụ kiện nội thất")]
    InteriorAccessories = 31,

    // 🚀 Công nghệ hỗ trợ lái xe
    [Display(Name = "Hệ thống điều khiển hành trình")]
    CruiseControl = 32,

    [Display(Name = "Camera và cảm biến đỗ xe")]
    ParkingAssist = 33,

    [Display(Name = "Hệ thống khởi động từ xa")]
    RemoteStart = 34,

    // 🛠 Bảo trì & bảo dưỡng
    [Display(Name = "Phụ tùng bảo dưỡng")]
    Maintenance = 35,

    [Display(Name = "Hệ thống chống ồn")]
    SoundDampening = 36,

    // 🌱 Hệ thống nhiên liệu tiên tiến (EV & Hybrid)
    [Display(Name = "Pin và mô-đun điện")]
    BatteryAndModules = 37,

    [Display(Name = "Bộ sạc và hệ thống quản lý pin")]
    ChargingSystem = 38,

    // 🛰 Hệ thống điều hướng & viễn thông
    [Display(Name = "Hệ thống viễn thông & Internet")]
    Telematics = 39,

    [Display(Name = "Màn hình hiển thị HUD")]
    HUD = 40,

    // 🏎 Hệ thống khí động học
    [Display(Name = "Cánh gió và bộ khuếch tán")]
    Aerodynamics = 41,

    // 🔇 Hệ thống cách âm & cách nhiệt
    [Display(Name = "Cách âm & chống rung")]
    SoundProofing = 42,

    [Display(Name = "Kính chống UV và cách nhiệt")]
    UVGlass = 43,

    // 🏕 Phụ kiện chuyên dụng
    [Display(Name = "Giá nóc và hộp chứa đồ")]
    RoofRack = 44,

    [Display(Name = "Bộ móc kéo xe")]
    TowHitch = 45,

    // ❓ Khác
    [Display(Name = "Phụ tùng khác")]
    Other = 255
}
