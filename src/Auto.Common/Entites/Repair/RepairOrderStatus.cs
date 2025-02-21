using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entites.Repair;

/// <summary>
/// Enum đại diện cho các trạng thái của đơn sửa chữa.
/// </summary>
public enum RepairOrderStatus
{
    None = 0,

    [Display(Name = "Chờ xác nhận")]
    Pending = 1,

    [Display(Name = "Đang chờ phụ tùng")]
    WaitingForParts = 2,

    [Display(Name = "Đang sửa chữa")]
    InProgress = 3,

    [Display(Name = "Hoàn thành (chưa thanh toán)")]
    Completed = 4,

    [Display(Name = "Đã thanh toán")]
    Paid = 5,

    [Display(Name = "Đã hủy")]
    Canceled = 6
}