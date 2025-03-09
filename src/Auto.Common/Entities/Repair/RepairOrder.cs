using Auto.Common.Entities.Bill;
using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Auto.Common.Entities.Repair;

/// <summary>
/// Lớp đại diện cho đơn sửa chữa.
/// </summary>
[Table(nameof(RepairOrder))]
public class RepairOrder
{
    #region Fields

    // Hiện tại không có private fields, để lại region này cho tính nhất quán.

    #endregion

    #region Identification Properties

    /// <summary>
    /// Mã đơn sửa chữa.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Id hóa đơn.
    /// </summary>
    [ForeignKey(nameof(Invoice))]
    public int InvoiceId { get; set; }

    /// <summary>
    /// Thông tin hóa đơn liên quan (Navigation Property).
    /// </summary>
    public virtual Invoice Invoice { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    [ForeignKey(nameof(Customer))]
    public int OwnerId { get; set; }

    /// <summary>
    /// Mã xe liên quan đến đơn sửa chữa.
    /// </summary>
    [ForeignKey(nameof(Vehicle))]
    public int CarId { get; set; }

    /// <summary>
    /// Thông tin xe liên quan (Navigation Property).
    /// </summary>
    public virtual Vehicle Vehicle { get; set; }

    /// <summary>
    /// Thông tin chủ xe (Navigation Property).
    /// </summary>
    public virtual Customer Owner { get; set; }

    #endregion

    #region Order Details Properties

    /// <summary>
    /// Ngày lập đơn.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public virtual ICollection<RepairTask> RepairTaskList { get; set; } = [];

    /// <summary>
    /// Danh sách phụ tùng thay thế liên quan.
    /// </summary>
    public virtual ICollection<RepairOrderSparePart> RepairOrderSpareParts { get; set; } = [];

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalRepairCost =>
        (RepairTaskList?.Sum(task => task.ServiceItem.UnitPrice) ?? 0) +
        (RepairOrderSpareParts?.Sum(sp => sp.SparePart.SellingPrice * sp.Quantity) ?? 0);

    #endregion
}