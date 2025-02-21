using Auto.Common.Entities.Suppliers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auto.Common.Entities.Part;

/// <summary>
/// Lớp đại diện cho phụ tùng.
/// </summary>
[Table(nameof(SparePart))]
public class SparePart
{
    private decimal _sellingPrice;
    private int _inventoryQuantity;

    /// <summary>
    /// Mã phụ tùng.
    /// </summary>
    [Key]
    public int PartId { get; set; }

    /// <summary>
    /// Id nhà cung cấp của phụ tùng.
    /// </summary>
    [ForeignKey(nameof(Suppliers.Supplier))]
    public int SupplierId { get; set; }

    /// <summary>
    /// Thông tin nhà cung cấp (Navigation Property).
    /// </summary>
    public virtual Supplier Supplier { get; set; }

    /// <summary>
    /// Loại phụ tùng.
    /// </summary>
    public PartCategory PartCategory { get; set; }

    /// <summary>
    /// Tên phụ tùng.
    /// </summary>
    [Required, StringLength(100, ErrorMessage = "Part name must not exceed 100 characters.")]
    public string PartName { get; set; } = string.Empty;

    /// <summary>
    /// Giá nhập phụ tùng.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// Giá bán của phụ tùng (luôn lớn hơn hoặc bằng giá nhập).
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than 0.")]
    public decimal SellingPrice
    {
        get => _sellingPrice;
        set
        {
            if (value < PurchasePrice)
                throw new ArgumentException("Selling price cannot be lower than purchase price.");
            _sellingPrice = value;
        }
    }

    /// <summary>
    /// Số lượng tồn kho của phụ tùng.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Inventory quantity must be a non-negative integer.")]
    public int InventoryQuantity
    {
        get => _inventoryQuantity;
        private set
        {
            if (value < 0)
                throw new ArgumentException("Inventory quantity cannot be negative.");
            _inventoryQuantity = value;
        }
    }

    /// <summary>
    /// Đánh dấu phụ tùng không còn bán.
    /// </summary>
    [Required]
    public bool IsDiscontinued { get; set; } = false;

    /// <summary>
    /// Điều chỉnh số lượng tồn kho (cộng/trừ một giá trị dương).
    /// </summary>
    /// <param name="quantity">Số lượng cần thay đổi.</param>
    /// <exception cref="ArgumentException">Nếu số lượng tồn kho bị âm.</exception>
    public void AdjustInventory(int quantity)
    {
        if (InventoryQuantity + quantity < 0)
            throw new ArgumentException("Inventory quantity cannot be negative.");

        InventoryQuantity += quantity;
    }
}