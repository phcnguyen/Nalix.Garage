using System.ComponentModel;

namespace Auto.Common.Entites.Payments;

/// <summary>
/// Xác định các phương thức thanh toán có thể sử dụng.
/// </summary>
public enum PaymentMethod : byte
{
    [Description("Không có phương thức thanh toán")]
    None = 0,

    /// <summary>
    /// Thanh toán bằng tiền mặt trực tiếp.
    /// - Khách hàng thanh toán bằng tiền mặt tại cửa hàng hoặc khi giao dịch trực tiếp.
    /// </summary>
    [Description("Tiền mặt")]
    Cash = 1,

    /// <summary>
    /// Thanh toán bằng chuyển khoản ngân hàng.
    /// - Khách hàng thực hiện giao dịch thông qua tài khoản ngân hàng.
    /// - Có thể mất một khoảng thời gian để xử lý.
    /// </summary>
    [Description("Chuyển khoản ngân hàng")]
    BankTransfer = 2,

    /// <summary>
    /// Thanh toán bằng thẻ tín dụng.
    /// - Hỗ trợ các loại thẻ Visa, MasterCard, JCB,...
    /// - Có thể yêu cầu phí giao dịch.
    /// </summary>
    [Description("Thẻ tín dụng")]
    CreditCard = 3,

    /// <summary>
    /// Thanh toán qua ví điện tử Momo.
    /// - Khách hàng quét mã QR hoặc chuyển khoản qua ứng dụng Momo.
    /// </summary>
    [Description("Ví điện tử Momo")]
    Momo = 4,

    /// <summary>
    /// Thanh toán qua ví điện tử ZaloPay.
    /// - Khách hàng có thể thanh toán thông qua ứng dụng ZaloPay.
    /// </summary>
    [Description("Ví điện tử ZaloPay")]
    ZaloPay = 5,

    /// <summary>
    /// VNPay - Cổng thanh toán.
    /// - Khách hàng có thể thanh toán thông qua cổng thanh toán VNPay.
    /// </summary>
    [Description("VNPay - Cổng thanh toán")]
    VNPay = 6,

    /// <summary>
    /// PayPal - Thanh toán quốc tế.
    /// - Hỗ trợ giao dịch quốc tế qua PayPal.
    /// </summary>
    [Description("PayPal - Thanh toán quốc tế")]
    PayPal = 7,

    /// <summary>
    /// Stripe - Thanh toán trực tuyến.
    /// - Khách hàng có thể thanh toán trực tuyến qua Stripe.
    /// </summary>
    [Description("Stripe - Thanh toán trực tuyến")]
    Stripe = 8,

    /// <summary>
    /// Apple Pay.
    /// - Khách hàng có thể thanh toán qua Apple Pay.
    /// </summary>
    [Description("Apple Pay")]
    ApplePay = 9,

    /// <summary>
    /// Google Pay.
    /// - Khách hàng có thể thanh toán qua Google Pay.
    /// </summary>
    [Description("Google Pay")]
    GooglePay = 10,

    /// <summary>
    /// Phương thức thanh toán khác.
    /// - Dành cho các phương thức thanh toán chưa được liệt kê.
    /// - Có thể bao gồm các ví điện tử khác hoặc thanh toán qua trung gian.
    /// </summary>
    [Description("Khác")]
    Other = 255
}