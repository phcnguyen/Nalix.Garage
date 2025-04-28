using System.ComponentModel.DataAnnotations;

namespace NalixGarage.Common.Entities.Employees;

/// <summary>
/// Đại diện cho các vị trí công việc trong hệ thống quản lý gara ô tô.
/// </summary>
public enum Position : byte
{
    [Display(Name = "Không xác định")]
    None = 0,

    [Display(Name = "Nhân viên học việc")]
    Apprentice = 1,

    [Display(Name = "Thợ rửa xe")]
    CarWasher = 2,

    [Display(Name = "Thợ điện ô tô")]
    AutoElectrician = 3,

    [Display(Name = "Thợ máy gầm")]
    UnderCarMechanic = 4,

    [Display(Name = "Thợ đồng")]
    BodyworkMechanic = 5,

    [Display(Name = "Kỹ thuật viên sửa chữa chung")]
    Technician = 6,

    [Display(Name = "Nhân viên tiếp nhận xe")]
    Receptionist = 7,

    [Display(Name = "Nhân viên tư vấn dịch vụ")]
    Advisor = 8,

    [Display(Name = "Nhân viên hỗ trợ kỹ thuật")]
    Support = 9,

    [Display(Name = "Nhân viên kế toán")]
    Accountant = 10,

    [Display(Name = "Quản lý gara")]
    Manager = 11,

    [Display(Name = "Nhân viên bảo trì thiết bị")]
    MaintenanceStaff = 12,

    [Display(Name = "Điều phối viên kho")]
    InventoryCoordinator = 13,

    [Display(Name = "Giám sát kho")]
    WarehouseSupervisor = 14,

    [Display(Name = "Thợ sơn xe")]
    Painter = 15,

    [Display(Name = "Chuyên viên chẩn đoán lỗi xe")]
    DiagnosticSpecialist = 16,

    [Display(Name = "Chuyên viên sửa chữa động cơ")]
    EngineSpecialist = 17,

    [Display(Name = "Chuyên viên sửa chữa hộp số")]
    TransmissionSpecialist = 18,

    [Display(Name = "Chuyên viên sửa chữa điều hòa ô tô")]
    ACSpecialist = 19,

    [Display(Name = "Thợ mài bề mặt xe")]
    Grinder = 20,

    [Display(Name = "Nhân viên bảo hiểm xe")]
    InsuranceStaff = 21,

    [Display(Name = "Nhân viên tư vấn phụ tùng")]
    PartsConsultant = 22,

    [Display(Name = "Nhân viên giao nhận xe")]
    VehicleDeliveryStaff = 23,

    [Display(Name = "Nhân viên vệ sinh gara")]
    CleaningStaff = 24,

    [Display(Name = "Nhân viên bảo vệ")]
    Security = 25,

    [Display(Name = "Nhân viên marketing")]
    MarketingStaff = 26,

    [Display(Name = "Nhân viên chăm sóc khách hàng")]
    CustomerService = 27,

    [Display(Name = "Giám đốc kỹ thuật")]
    TechnicalDirector = 28,

    [Display(Name = "Giám đốc dịch vụ")]
    ServiceDirector = 29,

    [Display(Name = "Giám đốc điều hành")]
    ExecutiveDirector = 30,

    [Display(Name = "Kỹ thuật viên điện tử và lập trình ô tô")]
    ElectronicsAndProgrammingTechnician = 31,

    [Display(Name = "Chuyên viên kiểm tra chất lượng xe")]
    QualityControlSpecialist = 32,

    [Display(Name = "Nhân viên đặt hàng phụ tùng")]
    PartsOrderingStaff = 33,

    [Display(Name = "Chuyên viên bảo hành xe")]
    WarrantySpecialist = 34,

    [Display(Name = "Nhân viên thu ngân")]
    Cashier = 35,

    [Display(Name = "Trưởng ca làm việc")]
    ShiftSupervisor = 36,

    [Display(Name = "Lái thử xe sau sửa chữa")]
    TestDriver = 37,

    [Display(Name = "Chuyên viên lốp xe")]
    TireSpecialist = 38,

    [Display(Name = "Kỹ thuật viên hệ thống thủy lực")]
    HydraulicTechnician = 39
}