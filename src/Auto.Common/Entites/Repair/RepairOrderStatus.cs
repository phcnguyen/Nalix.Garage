namespace Auto.Common.Entites.Repair;

/// <summary>
/// Enum đại diện cho các trạng thái của đơn sửa chữa.
/// </summary>
public enum RepairOrderStatus
{
    None = 0,

    /// <summary>
    /// Chờ xác nhận.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Đang chờ phụ tùng.
    /// </summary>
    WaitingForParts = 2,

    /// <summary>
    /// Đang sửa chữa.
    /// </summary>
    InProgress = 3,

    /// <summary>
    /// Hoàn thành nhưng chưa thanh toán.
    /// </summary>
    Completed = 4,

    /// <summary>
    /// Đã thanh toán.
    /// </summary>
    Paid = 5,

    /// <summary>
    /// Đơn đã bị hủy.
    /// </summary>
    Canceled = 6
}