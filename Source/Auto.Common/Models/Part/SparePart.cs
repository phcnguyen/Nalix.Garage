using Auto.Common.Models.Vehicles;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Part;

/// <summary>
/// Lớp đại diện cho phụ tùng.
/// </summary>
public class SparePart
{
    /// <summary>
    /// Mã phụ tùng.
    /// </summary>
    public int PartId { get; set; }

    /// <summary>
    /// Id nhà cung cấp của phụ tùng.
    /// </summary>
    public int SupplierId { get; set; }

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
    /// Giá bán của phụ tùng (phải lớn hơn hoặc bằng giá nhập).
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than 0.")]
    public decimal SellingPrice { get; set; }

    /// <summary>
    /// Số lượng tồn kho của phụ tùng.
    /// </summary>
    [Range(0, int.MaxValue, ErrorMessage = "Inventory quantity must be a non-negative integer.")]
    public int InventoryQuantity { get; set; }

    /// <summary>
    /// Đánh dấu phụ tùng không còn bán.
    /// </summary>
    public bool IsDiscontinued { get; set; } = false;

    /// <summary>
    /// Phụ tùng phù hợp với các hãng xe.
    /// </summary>
    public HashSet<CarBrand> CompatibleBrands { get; set; } = [];

    /// <summary>
    /// Phụ tùng dùng được cho các dòng xe cụ thể.
    /// </summary>
    public HashSet<string> CompatibleModels { get; set; } = [];

    /// <summary>
    /// Kiểm tra hợp lệ khi khởi tạo hoặc cập nhật phụ tùng.
    /// </summary>
    /// <returns>Danh sách lỗi (nếu có).</returns>
    public List<string> Validate()
    {
        List<string> errors = [];

        if (SellingPrice < PurchasePrice)
            errors.Add("Selling price cannot be lower than purchase price.");

        if (InventoryQuantity < 0)
            errors.Add("Inventory quantity cannot be negative.");

        return errors;
    }
}