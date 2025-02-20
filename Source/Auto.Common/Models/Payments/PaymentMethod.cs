namespace Auto.Common.Models.Payments;

/// <summary>
/// Xác định các phương thức thanh toán có thể sử dụng.
/// </summary>
public enum PaymentMethod
{
    /// <summary>
    /// Thanh toán bằng tiền mặt trực tiếp.
    /// - Khách hàng thanh toán bằng tiền mặt tại cửa hàng hoặc khi giao dịch trực tiếp.
    /// </summary>
    Cash,

    /// <summary>
    /// Thanh toán bằng chuyển khoản ngân hàng.
    /// - Khách hàng thực hiện giao dịch thông qua tài khoản ngân hàng.
    /// - Có thể mất một khoảng thời gian để xử lý.
    /// </summary>
    BankTransfer,

    /// <summary>
    /// Thanh toán bằng thẻ tín dụng.
    /// - Hỗ trợ các loại thẻ Visa, MasterCard, JCB,...
    /// - Có thể yêu cầu phí giao dịch.
    /// </summary>
    CreditCard,

    /// <summary>
    /// Thanh toán qua ví điện tử Momo.
    /// - Khách hàng quét mã QR hoặc chuyển khoản qua ứng dụng Momo.
    /// </summary>
    Momo,

    /// <summary>
    /// Thanh toán qua ví điện tử ZaloPay.
    /// - Khách hàng có thể thanh toán thông qua ứng dụng ZaloPay.
    /// </summary>
    ZaloPay,

    /// <summary>
    /// Phương thức thanh toán khác.
    /// - Dành cho các phương thức thanh toán chưa được liệt kê.
    /// - Có thể bao gồm các ví điện tử khác hoặc thanh toán qua trung gian.
    /// </summary>
    Other
}