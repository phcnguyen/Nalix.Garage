namespace Auto.Common.Models.Repair;

/// <summary>
/// Enum đại diện cho các trạng thái của đơn sửa chữa.
/// </summary>
public enum RepairOrderStatus
{
    /// <summary>
    /// Chờ xác nhận
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Đang sửa chữa
    /// </summary>
    InProgress = 2,

    /// <summary>
    /// Hoàn thành
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Đã thanh toán
    /// </summary>
    Paid = 4
}