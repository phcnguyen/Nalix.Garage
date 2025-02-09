namespace Auto.Common.Models.Bill.Transactions;

/// <summary>
/// Enum đại diện cho trạng thái giao dịch.
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    /// Đang xử lý.
    /// </summary>
    Pending,

    /// <summary>
    /// Thành công.
    /// </summary>
    Completed,

    /// <summary>
    /// Thất bại.
    /// </summary>
    Failed
}