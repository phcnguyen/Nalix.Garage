using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entites.Part;

/// <summary>
/// Lớp đại diện cho phụ tùng thay thế.
/// </summary>
[Table(nameof(ReplacementPart))]
public class ReplacementPart
{
    private int _quantity;
    private DateOnly? _expiryDate;

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
    [Required, StringLength(12, ErrorMessage = "Part code must not exceed 12 characters.")]
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
    public int Quantity
    {
        get => _quantity;
        private set
        {
            if (value < 0)
                throw new ArgumentException("Quantity cannot be negative.");
            _quantity = value;
        }
    }

    /// <summary>
    /// Đơn giá của phụ tùng.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Nhà sản xuất/nhãn hiệu phụ tùng.
    /// </summary>
    [Required, StringLength(75, ErrorMessage = "Manufacturer must not exceed 75 characters.")]
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Phụ tùng có bị lỗi hay không.
    /// </summary>
    [Required]
    public bool IsDefective { get; private set; } = false;

    /// <summary>
    /// Ngày hết hạn của phụ tùng (nếu có).
    /// </summary>
    public DateOnly? ExpiryDate
    {
        get => _expiryDate;
        set
        {
            if (value.HasValue && value.Value < DateAdded)
                throw new ArgumentException("Expiry date cannot be earlier than the date added.");
            _expiryDate = value;
        }
    }

    /// <summary>
    /// Tổng giá trị phụ tùng (Quantity * UnitPrice).
    /// </summary>
    public decimal TotalValue => Quantity * UnitPrice;

    /// <summary>
    /// Điều chỉnh số lượng phụ tùng (cộng/trừ một giá trị dương).
    /// </summary>
    /// <param name="amount">Số lượng cần thay đổi.</param>
    /// <exception cref="ArgumentException">Nếu số lượng bị âm.</exception>
    public void AdjustQuantity(int amount)
    {
        if (Quantity + amount < 0)
            throw new ArgumentException("Quantity cannot be negative.");
        Quantity += amount;
    }

    /// <summary>
    /// Đánh dấu phụ tùng là bị lỗi.
    /// </summary>
    public void MarkAsDefective() => IsDefective = true;
}