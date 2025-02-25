using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Entities.Transactions;

/// <summary>
/// Xác định các loại giao dịch tài chính trong hệ thống.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Giao dịch thu tiền từ khách hàng hoặc các nguồn khác.
    /// - Ví dụ: Thanh toán hóa đơn dịch vụ, bán phụ tùng.
    /// </summary>
    [Display(Name = "Thu tiền (doanh thu)")]
    Revenue = 1,

    /// <summary>
    /// Giao dịch chi tiền cho các khoản chi phí.
    /// - Ví dụ: Mua vật tư, trả lương nhân viên.
    /// </summary>
    [Display(Name = "Chi tiền (chi phí)")]
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
    RepairCost = 4
}