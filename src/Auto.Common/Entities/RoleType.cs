namespace Auto.Common.Entities;

/// <summary>
/// Enum đại diện cho các vai trò trong hệ thống.
/// </summary>
public enum RoleType : byte
{
    /// <summary>
    /// Vai trò người dùng thông thường (khách hàng hoặc nhân viên cơ bản).
    /// </summary>
    User = 1,

    /// <summary>
    /// Vai trò kế toán, quản lý các bút toán và báo cáo tài chính.
    /// </summary>
    Accountant = 2,

    /// <summary>
    /// Vai trò nhân viên kỹ thuật.
    /// </summary>
    Technique = 3,

    /// <summary>
    /// Vai trò quản trị viên có quyền truy cập toàn bộ hệ thống.
    /// </summary>
    Admin = 4,
}