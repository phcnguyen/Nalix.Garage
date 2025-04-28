using System.ComponentModel.DataAnnotations;

namespace NalixGarage.Common.Entities.Transactions;

/// <summary>
/// Xác định các loại giao dịch tài chính trong hệ thống.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Giao dịch thu tiền từ khách hàng hoặc các nguồn khác.
    /// - Ví dụ: Thanh toán hóa đơn dịch vụ, bán phụ tùng.
    /// </summary>
    [Display(Name = "Thu tiền")]
    Revenue = 1,

    /// <summary>
    /// Giao dịch chi tiền cho các khoản chi phí.
    /// - Ví dụ: Mua vật tư, trả lương nhân viên.
    /// </summary>
    [Display(Name = "Chi tiền")]
    Expense = 2,

    /// <summary>
    /// Giao dịch trả nợ, thanh toán các khoản vay hoặc công nợ.
    /// - Ví dụ: Thanh toán công nợ nhà cung cấp.
    /// </summary>
    [Display(Name = "Thanh toán công nợ")]
    DebtPayment = 3,

    /// <summary>
    /// Chi phí sửa chữa, bảo trì phương tiện hoặc thiết bị.
    /// - Ví dụ: Chi phí thay thế linh kiện, sửa chữa xe.
    /// </summary>
    [Display(Name = "Chi phí sửa chữa")]
    RepairCost = 4,

    /// <summary>
    /// Giao dịch tạm ứng tiền cho nhân viên hoặc các khoản chi chưa hoàn tất.
    /// </summary>
    [Display(Name = "Tạm ứng")]
    AdvancePayment = 5,

    /// <summary>
    /// Giao dịch hoàn tiền cho khách hàng.
    /// - Ví dụ: Hoàn tiền do lỗi dịch vụ, chính sách bảo hành.
    /// </summary>
    [Display(Name = "Hoàn tiền")]
    Refund = 6,

    /// <summary>
    /// Giao dịch chuyển tiền giữa các tài khoản nội bộ.
    /// - Ví dụ: Chuyển tiền từ quỹ tiền mặt sang tài khoản ngân hàng.
    /// </summary>
    [Display(Name = "Chuyển khoản nội bộ")]
    InternalTransfer = 7,

    /// <summary>
    /// Thu tiền đặt cọc từ khách hàng.
    /// - Ví dụ: Khách hàng đặt cọc cho dịch vụ lớn hoặc mua hàng trước.
    /// </summary>
    [Display(Name = "Tiền đặt cọc")]
    Deposit = 8
}
