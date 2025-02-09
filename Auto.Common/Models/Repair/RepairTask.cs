namespace Auto.Common.Models.Repair;

/// <summary>
/// Lớp đại diện cho công việc sửa chữa.
/// </summary>
public class RepairTask
{
    /// <summary>
    /// Mô tả công việc sửa chữa.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Đơn giá của công việc sửa chữa.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Nhân viên thực hiện công việc sửa chữa.
    /// </summary>
    public Employee PerformingEmployee { get; set; }
}