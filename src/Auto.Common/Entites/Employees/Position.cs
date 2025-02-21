using System.ComponentModel;

namespace Auto.Common.Entites.Employees;

/// <summary>
/// Đại diện cho các vị trí công việc trong hệ thống quản lý gara ô tô.
/// </summary>
public enum Position : byte
{
    [Description("Không xác định")]
    None = 0,

    [Description("Nhân viên học việc")]
    Apprentice = 1,

    [Description("Thợ rửa xe")]
    CarWasher = 2,

    [Description("Thợ điện ô tô")]
    AutoElectrician = 3,

    [Description("Thợ máy gầm")]
    UnderCarMechanic = 4,

    [Description("Thợ đồng")]
    BodyworkMechanic = 5,

    [Description("Kỹ thuật viên sửa chữa chung")]
    Technician = 6,

    [Description("Nhân viên tiếp nhận xe")]
    Receptionist = 7,

    [Description("Nhân viên tư vấn dịch vụ")]
    Advisor = 8,

    [Description("Nhân viên hỗ trợ kỹ thuật")]
    Support = 9,

    [Description("Nhân viên kế toán")]
    Accountant = 10,

    [Description("Quản lý gara")]
    Manager = 11,

    [Description("Nhân viên bảo trì thiết bị")]
    MaintenanceStaff = 12,

    [Description("Điều phối viên kho")]
    InventoryCoordinator = 13,

    [Description("Giám sát kho")]
    WarehouseSupervisor = 14,

    [Description("Thợ sơn xe")]
    Painter = 15,

    [Description("Chuyên viên chẩn đoán lỗi xe")]
    DiagnosticSpecialist = 16,

    [Description("Chuyên viên sửa chữa động cơ")]
    EngineSpecialist = 17,

    [Description("Chuyên viên sửa chữa hộp số")]
    TransmissionSpecialist = 18,

    [Description("Chuyên viên sửa chữa điều hòa ô tô")]
    ACSpecialist = 19,

    [Description("Thợ mài bề mặt xe")]
    Grinder = 20,

    [Description("Nhân viên bảo hiểm xe")]
    InsuranceStaff = 21,

    [Description("Nhân viên tư vấn phụ tùng")]
    PartsConsultant = 22,

    [Description("Nhân viên giao nhận xe")]
    VehicleDeliveryStaff = 23,

    [Description("Nhân viên vệ sinh gara")]
    CleaningStaff = 24,

    [Description("Nhân viên bảo vệ")]
    Security = 25,

    [Description("Nhân viên marketing")]
    MarketingStaff = 26,

    [Description("Nhân viên chăm sóc khách hàng")]
    CustomerService = 27,

    [Description("Giám đốc kỹ thuật")]
    TechnicalDirector = 28,

    [Description("Giám đốc dịch vụ")]
    ServiceDirector = 29,

    [Description("Giám đốc điều hành")]
    ExecutiveDirector = 30,

    [Description("Kỹ thuật viên điện tử và lập trình ô tô")]
    ElectronicsAndProgrammingTechnician = 31,

    [Description("Chuyên viên kiểm tra chất lượng xe")]
    QualityControlSpecialist = 32,

    [Description("Nhân viên đặt hàng phụ tùng")]
    PartsOrderingStaff = 33,

    [Description("Chuyên viên bảo hành xe")]
    WarrantySpecialist = 34,

    [Description("Nhân viên thu ngân")]
    Cashier = 35,

    [Description("Trưởng ca làm việc")]
    ShiftSupervisor = 36,

    [Description("Lái thử xe sau sửa chữa")]
    TestDriver = 37,

    [Description("Chuyên viên lốp xe")]
    TireSpecialist = 38,

    [Description("Kỹ thuật viên hệ thống thủy lực")]
    HydraulicTechnician = 39
}