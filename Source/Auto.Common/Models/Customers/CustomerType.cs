namespace Auto.Common.Models.Customers;

/// <summary>
/// Enum đại diện cho loại khách hàng.
/// </summary>
public enum CustomerType
{
    /// <summary>
    /// Không xác định hoặc chưa được phân loại.
    /// </summary>
    Unknown = 0,

    /// <summary>
    /// Cá nhân.
    /// </summary>
    Individual = 1,

    /// <summary>
    /// Doanh nghiệp.
    /// </summary>
    Business = 2,

    /// <summary>
    /// Chính phủ.
    /// </summary>
    Government = 3,

    /// <summary>
    /// Loại khách hàng khác.
    /// </summary>
    Other = 4
}