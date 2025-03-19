namespace Auto.Common.Dto;

/// <summary>
/// Yêu cầu tìm kiếm khách hàng.
/// </summary>
public sealed class SearchDto
{
    /// <summary>
    /// Từ khóa dùng để tìm kiếm khách hàng.  
    /// Có thể là tên, số điện thoại hoặc thông tin liên quan khác.
    /// </summary>
    public string Keyword { get; set; } = string.Empty;

    /// <summary>
    /// Chỉ mục của trang cần lấy.  
    /// Bắt đầu từ 0 cho trang đầu tiên.
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// Số lượng bản ghi trên mỗi trang.  
    /// Giá trị khuyến nghị: 10, 20, 50, v.v.
    /// </summary>
    public int PageSize { get; set; }
}
