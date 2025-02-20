using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Part;

public enum PartCategory
{
    None,

    [Display(Name = "Phụ tùng động cơ")]
    Engine,

    [Display(Name = "Phụ tùng phanh")]
    Brake,

    [Display(Name = "Phụ tùng truyền động")]
    Transmission,

    [Display(Name = "Phụ tùng điện")]
    Electrical,

    [Display(Name = "Phụ tùng thân xe")]
    Body,

    [Display(Name = "Hệ thống treo")]
    Suspension,

    [Display(Name = "Hệ thống làm mát")]
    Cooling,

    [Display(Name = "Hệ thống nhiên liệu")]
    Fuel,

    [Display(Name = "Hệ thống xả")]
    Exhaust,

    [Display(Name = "Hệ thống điều hòa")]
    AirConditioning,

    [Display(Name = "Hệ thống lái")]
    Steering,

    [Display(Name = "Bánh xe và lốp")]
    WheelAndTire,

    [Display(Name = "Nội thất")]
    Interior,

    [Display(Name = "Phụ tùng bảo dưỡng")]
    Maintenance,

    [Display(Name = "Hệ thống an toàn")]
    Safety,

    [Display(Name = "Hệ thống khí thải")]
    Emissions,

    [Display(Name = "Hệ thống đánh lửa")]
    Ignition,

    [Display(Name = "Hệ thống phun nhiên liệu")]
    FuelInjection,

    [Display(Name = "Bộ tăng áp")]
    Turbocharger,

    [Display(Name = "Hệ thống bôi trơn")]
    Lubrication,

    [Display(Name = "Gương và kính")]
    MirrorsAndGlass,

    [Display(Name = "Hệ thống chiếu sáng")]
    Lighting,

    [Display(Name = "Hệ thống chống ồn")]
    SoundDampening,

    [Display(Name = "Cảm biến và mô-đun điều khiển")]
    SensorsAndModules,

    [Display(Name = "Hệ thống chống bó cứng phanh")]
    ABS,

    [Display(Name = "Hệ thống ổn định điện tử")]
    ESC,

    [Display(Name = "Túi khí và các thiết bị an toàn")]
    Airbags,

    [Display(Name = "Hệ thống giải trí")]
    Entertainment,

    [Display(Name = "Hệ thống định vị")]
    Navigation,

    [Display(Name = "Hệ thống sưởi ghế")]
    SeatHeating,

    [Display(Name = "Hệ thống làm mát ghế")]
    SeatCooling,

    [Display(Name = "Phụ kiện ngoại thất")]
    ExteriorAccessories,

    [Display(Name = "Phụ kiện nội thất")]
    InteriorAccessories,

    [Display(Name = "Hệ thống khóa và an ninh")]
    SecurityAndLocking,

    [Display(Name = "Hệ thống điều khiển hành trình")]
    CruiseControl,

    [Display(Name = "Camera và cảm biến đỗ xe")]
    ParkingAssist,

    [Display(Name = "Hệ thống khởi động từ xa")]
    RemoteStart,

    [Display(Name = "Phụ tùng khác")]
    Other
}