namespace Auto.Common.Models.Bill.Transactions;

/// <summary>
/// Phương thức thanh toán.
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Tiền mặt.
    /// </summary>
    Cash,

    /// <summary>
    /// Chuyển khoản.
    /// </summary>
    BankTransfer,

    /// <summary>
    /// Thẻ tín dụng.
    /// </summary>
    CreditCard,

    /// <summary>
    /// Momo.
    /// </summary>
    Momo,

    /// <summary>
    /// ZaloPay.
    /// </summary>
    ZaloPay,

    /// <summary>
    /// Khác.
    /// </summary>
    Other
}