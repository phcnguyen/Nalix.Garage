namespace Auto.Common.Models.Bill.Transactions;

/// <summary>
/// Liệt kê các loại giao dịch.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Giao dịch thu.
    /// </summary>
    Revenue = 1,

    /// <summary>
    /// Giao dịch chi.
    /// </summary>
    Expense = 2,

    /// <summary>
    /// Giao dịch trả nợ.
    /// </summary>
    DebtPayment = 3,

    /// <summary>
    /// Giao dịch chi phí sửa chữa.
    /// </summary>
    RepairCost = 4
}