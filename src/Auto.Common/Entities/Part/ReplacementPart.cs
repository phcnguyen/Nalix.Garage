using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace Auto.Common.Entities.Part;

/// <summary>
/// Lớp đại diện cho phụ tùng thay thế.
/// </summary>
[Table(nameof(ReplacementPart))]
public class ReplacementPart
{
    #region Fields

    private int _quantity;
    private string _partName = string.Empty;
    private DateOnly _dateAdded = DateOnly.FromDateTime(DateTime.UtcNow);
    private DateOnly? _expiryDate;
    private readonly Lock _lock = new();

    #endregion

    #region Constructor

    public ReplacementPart()
    {
        if (ExpiryDate.HasValue && ExpiryDate.Value < DateAdded)
            throw new ArgumentException("Expiry date cannot be earlier than the date added.");
    }

    #endregion

    #region Identification Properties

    /// <summary>
    /// Mã phụ tùng thay thế.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Mã SKU (Stock Keeping Unit) hoặc mã phụ tùng.
    /// </summary>
    [Required, StringLength(12, ErrorMessage = "Part code must not exceed 12 characters.")]
    [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Part code must contain only letters and numbers.")]
    public string PartCode { get; set; } = string.Empty;

    /// <summary>
    /// Tên phụ tùng.
    /// </summary>
    [Required, StringLength(100, ErrorMessage = "Part name must not exceed 100 characters.")]
    public string PartName
    {
        get => _partName;
        set => _partName = value ?? throw new ArgumentNullException(nameof(PartName));
    }

    /// <summary>
    /// Nhà sản xuất/nhãn hiệu phụ tùng.
    /// </summary>
    [Required, StringLength(75, ErrorMessage = "Manufacturer must not exceed 75 characters.")]
    public string Manufacturer { get; set; } = string.Empty;

    #endregion

    #region Quantity and Value Properties

    /// <summary>
    /// Số lượng phụ tùng trong kho.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Quantity must be at least zero.")]
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0)
                throw new ArgumentException("Quantity cannot be negative.");
            _quantity = value;
        }
    }

    /// <summary>
    /// Đơn giá của phụ tùng.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Tổng giá trị phụ tùng (Quantity * UnitPrice).
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalValue => Quantity * UnitPrice;

    #endregion

    #region Status Properties

    /// <summary>
    /// Kiểm tra xem phụ tùng còn hàng hay không.
    /// </summary>
    public bool IsInStock => Quantity > 0;

    /// <summary>
    /// Phụ tùng có bị lỗi hay không.
    /// </summary>
    [Required]
    public bool IsDefective { get; private set; } = false;

    #endregion

    #region Date Properties

    /// <summary>
    /// Ngày nhập kho.
    /// </summary>
    public DateOnly DateAdded
    {
        get => _dateAdded;
        set
        {
            _dateAdded = value;
            ValidateExpiryDate();
        }
    }

    /// <summary>
    /// Ngày hết hạn của phụ tùng (nếu có).
    /// </summary>
    public DateOnly? ExpiryDate
    {
        get => _expiryDate;
        set
        {
            _expiryDate = value;
            ValidateExpiryDate();
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Tăng số lượng phụ tùng trong kho.
    /// </summary>
    /// <param name="amount">Số lượng cần tăng.</param>
    /// <exception cref="ArgumentException">Thrown when amount is less than or equal to zero.</exception>
    public void IncreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Increase amount must be positive.");
        lock (_lock)
        {
            Quantity += amount;
        }
    }

    /// <summary>
    /// Giảm số lượng phụ tùng trong kho.
    /// </summary>
    /// <param name="amount">Số lượng cần giảm.</param>
    /// <exception cref="ArgumentException">Thrown when amount is invalid or insufficient stock.</exception>
    public void DecreaseQuantity(int amount)
    {
        if (amount <= 0) throw new ArgumentException("Decrease amount must be positive.");
        if (Quantity < amount) throw new ArgumentException("Not enough stock.");
        lock (_lock)
        {
            Quantity -= amount;
        }
    }

    /// <summary>
    /// Đánh dấu phụ tùng là bị lỗi.
    /// </summary>
    public void MarkAsDefective() => IsDefective = true;

    /// <summary>
    /// Hủy trạng thái lỗi của phụ tùng (nếu cần).
    /// </summary>
    public void UnmarkAsDefective() => IsDefective = false;

    #endregion

    #region Private Methods

    private void ValidateExpiryDate()
    {
        if (ExpiryDate.HasValue && ExpiryDate.Value < DateAdded)
            throw new ArgumentException("Expiry date cannot be earlier than the date added.");
    }

    #endregion
}