namespace Auto.Common.Models.Employees;

/// <summary>
/// Đại diện cho các vị trí công việc trong hệ thống quản lý gara ô tô.
/// Được sử dụng để phân loại nhân viên theo vai trò cụ thể.
/// </summary>
public enum Position
{
    /// <summary>
    /// Không có vị trí cụ thể.
    /// Sử dụng khi nhân viên chưa được phân công vị trí.
    /// </summary>
    None = 0,

    /// <summary>
    /// Nhân viên học việc, đang trong quá trình đào tạo.
    /// </summary>
    Apprentice = 1,

    /// <summary>
    /// Thợ rửa xe, chịu trách nhiệm vệ sinh xe ô tô.
    /// </summary>
    CarWasher = 2,

    /// <summary>
    /// Thợ điện ô tô, chuyên sửa chữa hệ thống điện của xe.
    /// </summary>
    AutoElectrician = 3,

    /// <summary>
    /// Thợ máy gầm, chuyên bảo trì và sửa chữa hệ thống khung gầm xe.
    /// </summary>
    UnderCarMechanic = 4,

    /// <summary>
    /// Thợ đồng, chịu trách nhiệm sửa chữa khung vỏ xe.
    /// </summary>
    BodyworkMechanic = 5,

    /// <summary>
    /// Kỹ thuật viên sửa chữa chung.
    /// </summary>
    Technician = 6,

    /// <summary>
    /// Nhân viên tiếp nhận xe và làm thủ tục cho khách hàng.
    /// </summary>
    Receptionist = 7,

    /// <summary>
    /// Nhân viên tư vấn dịch vụ sửa chữa cho khách hàng.
    /// </summary>
    Advisor = 8,

    /// <summary>
    /// Nhân viên hỗ trợ kỹ thuật hoặc khách hàng.
    /// </summary>
    Support = 9,

    /// <summary>
    /// Nhân viên kế toán, quản lý tài chính và thu chi.
    /// </summary>
    Accountant = 10,

    /// <summary>
    /// Quản lý gara, giám sát hoạt động kinh doanh và kỹ thuật.
    /// </summary>
    Manager = 11,

    /// <summary>
    /// Nhân viên bảo trì trang thiết bị của gara.
    /// </summary>
    MaintenanceStaff = 12,

    /// <summary>
    /// Điều phối viên kho, chịu trách nhiệm quản lý xuất nhập phụ tùng.
    /// </summary>
    InventoryCoordinator = 13,

    /// <summary>
    /// Giám sát kho, kiểm soát hàng tồn kho và quy trình kho bãi.
    /// </summary>
    WarehouseSupervisor = 14,

    /// <summary>
    /// Thợ sơn xe, chuyên về sơn và hoàn thiện bề mặt xe.
    /// </summary>
    Painter = 15,

    /// <summary>
    /// Chuyên viên chẩn đoán, sử dụng thiết bị kiểm tra lỗi trên xe.
    /// </summary>
    DiagnosticSpecialist = 16,

    /// <summary>
    /// Chuyên viên sửa chữa và bảo trì động cơ xe.
    /// </summary>
    EngineSpecialist = 17,

    /// <summary>
    /// Chuyên viên sửa chữa và bảo trì hộp số xe.
    /// </summary>
    TransmissionSpecialist = 18,

    /// <summary>
    /// Chuyên viên sửa chữa hệ thống điều hòa ô tô.
    /// </summary>
    ACSpecialist = 19,

    /// <summary>
    /// Thợ mài, chuyên gia xử lý bề mặt kim loại trước khi sơn.
    /// </summary>
    Grinder = 20,

    /// <summary>
    /// Nhân viên bảo hiểm, hỗ trợ xử lý thủ tục bảo hiểm xe.
    /// </summary>
    InsuranceStaff = 21,

    /// <summary>
    /// Nhân viên tư vấn và bán phụ tùng xe.
    /// </summary>
    PartsConsultant = 22,

    /// <summary>
    /// Nhân viên giao nhận xe sau khi sửa chữa hoặc bảo dưỡng.
    /// </summary>
    VehicleDeliveryStaff = 23,

    /// <summary>
    /// Nhân viên vệ sinh, duy trì môi trường sạch sẽ trong gara.
    /// </summary>
    CleaningStaff = 24,

    /// <summary>
    /// Nhân viên bảo vệ, đảm bảo an ninh cho gara.
    /// </summary>
    Security = 25,

    /// <summary>
    /// Nhân viên marketing, thực hiện quảng bá và tiếp thị dịch vụ gara.
    /// </summary>
    MarketingStaff = 26,

    /// <summary>
    /// Nhân viên chăm sóc khách hàng, hỗ trợ giải đáp thắc mắc.
    /// </summary>
    CustomerService = 27,

    /// <summary>
    /// Giám đốc kỹ thuật, phụ trách các vấn đề liên quan đến công nghệ và sửa chữa.
    /// </summary>
    TechnicalDirector = 28,

    /// <summary>
    /// Giám đốc dịch vụ, quản lý dịch vụ khách hàng và kỹ thuật.
    /// </summary>
    ServiceDirector = 29,

    /// <summary>
    /// Giám đốc điều hành, quản lý toàn bộ hoạt động của gara.
    /// </summary>
    ExecutiveDirector = 30,

    /// <summary>
    /// Thợ điện tử và lập trình, chuyên sửa chữa hệ thống điện tử ô tô.
    /// </summary>
    ElectronicsAndProgrammingTechnician = 31,

    /// <summary>
    /// Chuyên viên kiểm tra chất lượng, đảm bảo tiêu chuẩn sửa chữa.
    /// </summary>
    QualityControlSpecialist = 32,

    /// <summary>
    /// Nhân viên đặt hàng phụ tùng, quản lý việc nhập hàng từ nhà cung cấp.
    /// </summary>
    PartsOrderingStaff = 33,

    /// <summary>
    /// Chuyên viên bảo hành, xử lý các yêu cầu bảo hành xe.
    /// </summary>
    WarrantySpecialist = 34,

    /// <summary>
    /// Nhân viên thu ngân, xử lý thanh toán của khách hàng.
    /// </summary>
    Cashier = 35,

    /// <summary>
    /// Trưởng ca, giám sát hoạt động trong ca làm việc.
    /// </summary>
    ShiftSupervisor = 36
}