using System.ComponentModel.DataAnnotations;

namespace NalixGarage.Common.Entities.Repair;

/// <summary>
/// Enum đại diện cho các trạng thái của đơn sửa chữa.
/// </summary>
public enum RepairOrderStatus
{
    [Display(Name = "Không xác định")]
    None = 0,

    [Display(Name = "Chờ xác nhận")]
    Pending = 1,

    [Display(Name = "Đang kiểm tra xe")]
    Inspecting = 2,  // 🚗 Giai đoạn kiểm tra ban đầu

    [Display(Name = "Chờ báo giá")]
    QuotationPending = 3,  // 💰 Chờ khách duyệt báo giá

    [Display(Name = "Khách hàng từ chối sửa chữa")]
    RejectedByCustomer = 4,  // ❌ Khách từ chối sau khi báo giá

    [Display(Name = "Đang chờ phụ tùng")]
    WaitingForParts = 5,

    [Display(Name = "Đang sửa chữa")]
    InProgress = 6,

    [Display(Name = "Chờ kiểm tra sau sửa chữa")]
    PostRepairInspection = 7,  // ✅ Kiểm tra lần cuối trước khi bàn giao

    [Display(Name = "Hoàn thành (chưa thanh toán)")]
    Completed = 8,

    [Display(Name = "Đã thanh toán")]
    Paid = 9,

    [Display(Name = "Bị từ chối bảo hiểm")]
    InsuranceRejected = 10,  // 🚨 Bảo hiểm không duyệt

    [Display(Name = "Đã hủy")]
    Canceled = 11
}
