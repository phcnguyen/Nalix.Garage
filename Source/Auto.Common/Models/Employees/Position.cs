namespace Auto.Common.Models.Employees;

/// <summary>
/// Enum định nghĩa các vị trí công việc của nhân viên.
/// </summary>
public enum Position
{
    /// <summary>
    /// Vị trí không xác định.
    /// </summary>
    None,

    /// <summary>
    /// Vị trí học việc.
    /// </summary>
    Apprentice = 1,

    /// <summary>
    /// Vị trí thợ rửa xe.
    /// </summary>
    CarWasher = 2,

    /// <summary>
    /// Vị trí thợ điện ô tô.
    /// </summary>
    AutoElectrician = 3,

    /// <summary>
    /// Vị trí thợ máy gầm.
    /// </summary>
    UnderCarMechanic = 4,

    /// <summary>
    /// Vị trí thợ đồng.
    /// </summary>
    BodyworkMechanic = 5,

    /// <summary>
    /// Vị trí nhân viên kỹ thuật sửa chữa.
    /// </summary>
    Technician = 6,

    /// <summary>
    /// Vị trí nhân viên tiếp nhận.
    /// </summary>
    Receptionist = 7,

    /// <summary>
    /// Vị trí kế toán.
    /// </summary>
    Accountant = 8,

    /// <summary>
    /// Vị trí quản lý.
    /// </summary>
    Manager = 9,

    /// <summary>
    /// Vị trí nhân viên bảo trì.
    /// </summary>
    MaintenanceStaff = 10
}