namespace Auto.Common.Models;

/// <summary>
/// Lớp đại diện cho giao dịch.
/// </summary>
public class Transaction
{
    /// <summary>
    /// Loại giao dịch.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Số tiền giao dịch.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Mô tả giao dịch.
    /// </summary>
    public string Description { get; set; }
}