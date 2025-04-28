using Nalix.Garage.Common.Entities.Suppliers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nalix.Garage.Common.Entities.Part;

/// <summary>
/// Lớp đại diện cho phụ tùng.
/// </summary>
[Table(nameof(SparePart))]
public class SparePart
{
    #region Fields

    private decimal _sellingPrice;
    private int _inventoryQuantity;

    #endregion

    #region Identification Properties

    /// <summary>
    /// Mã phụ tùng.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Id nhà cung cấp của phụ tùng.
    /// </summary>
    [ForeignKey(nameof(Supplier))]
    public int SupplierId { get; set; }

    /// <summary>
    /// Thông tin nhà cung cấp (Navigation Property).
    /// </summary>
    public virtual Supplier Supplier { get; set; }

    #endregion

    #region Basic Properties

    /// <summary>
    /// Loại phụ tùng.
    /// </summary>
    public PartCategory PartCategory { get; set; }

    /// <summary>
    /// Tên phụ tùng.
    /// </summary>
    [Required, StringLength(100, ErrorMessage = "Part name must not exceed 100 characters.")]
    public string PartName { get; set; } = string.Empty;

    #endregion

    #region Price and Quantity Properties

    /// <summary>
    /// Giá nhập phụ tùng.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Purchase price must be greater than 0.")]
    public decimal PurchasePrice { get; set; }

    /// <summary>
    /// Giá bán của phụ tùng (luôn lớn hơn hoặc bằng giá nhập).
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than 0.")]
    public decimal SellingPrice
    {
        get => _sellingPrice;
        set
        {
            if (value < PurchasePrice)
                throw new InvalidOperationException("Selling price cannot be lower than purchase price.");
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
        set
        {
            if (value < 0)
                throw new ArgumentException("Inventory quantity cannot be negative.");
            _inventoryQuantity = value;
        }
    }

    #endregion

    #region Status Properties

    /// <summary>
    /// Đánh dấu phụ tùng không còn bán.
    /// </summary>
    [Required]
    public bool IsDiscontinued { get; set; } = false;

    #endregion
}