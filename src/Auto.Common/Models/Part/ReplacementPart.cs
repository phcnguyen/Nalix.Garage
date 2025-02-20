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
    [Key]
    public int PartId { get; set; }

    /// <summary>
    /// Ngày nhập kho.
    /// </summary>
    public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    /// <summary>
    /// Mã SKU (Stock Keeping Unit) hoặc mã phụ tùng.
    /// </summary>
    [Required, StringLength(100, ErrorMessage = "Part code must not exceed 100 characters.")]
    public string PartCode { get; set; } = string.Empty;

    /// <summary>
    /// Tên phụ tùng.
    /// </summary>
    [Required, StringLength(100, ErrorMessage = "Part name must not exceed 100 characters.")]
    public string PartName { get; set; } = string.Empty;

    /// <summary>
    /// Số lượng phụ tùng trong kho.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least zero.")]
    public int Quantity { get; set; } = 0;

    /// <summary>
    /// Đơn giá của phụ tùng.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Nhà sản xuất/nhãn hiệu phụ tùng.
    /// </summary>
    [Required, StringLength(100, ErrorMessage = "Manufacturer must not exceed 100 characters.")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Phụ tùng có bị lỗi hay không.
    /// </summary>
    public bool IsDefective { get; set; } = false;

    /// <summary>
    /// Ngày hết hạn của phụ tùng (nếu có).
    /// </summary>
    public DateOnly? ExpiryDate { get; set; }

    /// <summary>
    /// Tổng giá trị phụ tùng (Quantity * UnitPrice).
    /// </summary>
    public decimal TotalValue() => Quantity * UnitPrice;
}