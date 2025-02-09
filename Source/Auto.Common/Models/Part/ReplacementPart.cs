using System;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Part;

/// <summary>
/// Lớp đại diện cho phụ tùng thay thế.
/// </summary>
public class ReplacementPart
{
    /// <summary>
    /// Mã phụ tùng thay thế.
    /// </summary>
    public int PartId { get; set; }

    /// <summary>
    /// Theo dõi thay đổi trong kho.
    /// </summary>
    public DateTime DateAdded { get; set; } = DateTime.Now;

    /// <summary>
    /// Phụ tùng có mã hoặc SKU (Stock Keeping Unit).
    /// </summary>
    [StringLength(100, ErrorMessage = "Part name must not exceed 100 characters.")]
    public string PartCode { get; set; } = string.Empty;

    /// <summary>
    /// Tên phụ tùng.
    /// </summary>
    [StringLength(100, ErrorMessage = "Part name must not exceed 100 characters.")]
    public string PartName { get; set; }

    /// <summary>
    /// Số lượng phụ tùng.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    /// <summary>
    /// Đơn giá của phụ tùng.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Thông tin nhà sản xuất/nhãn hiệu.
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Nếu phụ tùng có lỗi xử lý việc trả lại hàng.
    /// </summary>
    public bool IsReturned { get; set; } = false;

    /// <summary>
    /// Nếu phụ tùng có hạn sử dụng.
    /// </summary>
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Tổng giá trị phụ tùng thay thế (Quantity * UnitPrice).
    /// </summary>
    public decimal TotalPrice => Quantity * UnitPrice;
}