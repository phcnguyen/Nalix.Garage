namespace Auto.Common.Models.Bill;

public enum DiscountType
{
    /// <summary>
    /// No discount
    /// </summary>
    None,

    /// <summary>
    /// Discount is a percentage
    /// </summary>
    Percentage,

    /// <summary>
    ///  Discount is a fixed amount
    /// </summary>
    Amount
}