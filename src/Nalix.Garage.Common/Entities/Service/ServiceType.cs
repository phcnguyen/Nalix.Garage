using System.ComponentModel.DataAnnotations;

namespace Nalix.Garage.Common.Entities.Service;

/// <summary>
/// Enum đại diện cho các loại dịch vụ trong gara ô tô.
/// </summary>
public enum ServiceType : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    // 🔧 **Bảo trì & bảo dưỡng**
    [Display(Name = "Bảo dưỡng định kỳ")]
    Maintenance = 1,

    [Display(Name = "Kiểm tra xe")]
    Inspection = 2,

    [Display(Name = "Thay dầu & bộ lọc")]
    OilChange = 3,

    [Display(Name = "Dịch vụ lốp xe (Thay, vá, cân bằng)")]
    TireService = 4,

    [Display(Name = "Cân chỉnh góc đặt bánh xe (Alignment)")]
    WheelAlignment = 5,

    [Display(Name = "Dịch vụ điều hòa không khí")]
    ACService = 6,

    // 🚗 **Sửa chữa chung**
    [Display(Name = "Dịch vụ sửa chữa")]
    Repair = 10,

    [Display(Name = "Sửa chữa động cơ")]
    EngineRepair = 11,

    [Display(Name = "Sửa chữa hộp số & truyền động")]
    TransmissionRepair = 12,

    [Display(Name = "Sửa chữa hệ thống phanh")]
    BrakeRepair = 13,

    [Display(Name = "Sửa chữa hệ thống lái & treo")]
    SuspensionRepair = 14,

    [Display(Name = "Sửa chữa hệ thống nhiên liệu")]
    FuelSystemRepair = 15,

    [Display(Name = "Dịch vụ điện & ắc quy")]
    ElectricalService = 16,

    [Display(Name = "Sửa chữa hệ thống đánh lửa")]
    IgnitionRepair = 17,

    // 🎨 **Làm đẹp & phục hồi xe**
    [Display(Name = "Rửa xe & chăm sóc nội thất")]
    CarWashAndDetailing = 20,

    [Display(Name = "Sơn & làm đẹp xe")]
    Painting = 21,

    [Display(Name = "Phục hồi đèn pha & kính xe")]
    HeadlightRestoration = 22,

    [Display(Name = "Dán phim cách nhiệt & bảo vệ sơn")]
    WindowTintingAndPPF = 23,

    [Display(Name = "Dịch vụ phủ ceramic & nano coating")]
    CeramicCoating = 24,

    // 🛡 **Dịch vụ an toàn & kiểm định**
    [Display(Name = "Dịch vụ kiểm định xe")]
    VehicleInspection = 30,

    [Display(Name = "Kiểm tra & lắp đặt camera hành trình")]
    DashcamInstallation = 31,

    [Display(Name = "Lắp đặt & sửa chữa hệ thống cảm biến hỗ trợ lái")]
    ParkingSensorAndADAS = 32,

    // 🚨 **Dịch vụ khẩn cấp**
    [Display(Name = "Dịch vụ cứu hộ xe khẩn cấp")]
    EmergencyRoadsideAssistance = 40,

    [Display(Name = "Dịch vụ kéo xe")]
    TowingService = 41,

    [Display(Name = "Hỗ trợ khởi động xe (Nhảy bình)")]
    JumpStartService = 42,

    [Display(Name = "Hỗ trợ mở khóa xe")]
    LockoutAssistance = 43,

    [Display(Name = "Cung cấp nhiên liệu khẩn cấp")]
    EmergencyFuelDelivery = 44,

    [Display(Name = "Khác")]
    Other = 255
}
