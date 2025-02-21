using Auto.Common.Entites.Bill;
using Auto.Common.Entites.Customers;
using Auto.Common.Entites.Part;
using Auto.Common.Entites.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Auto.Common.Entites.Repair;

/// <summary>
/// Lớp đại diện cho đơn sửa chữa.
/// </summary>
[Table(nameof(RepairOrder))]
public class RepairOrder
{
    /// <summary>
    /// Mã đơn sửa chữa.
    /// </summary>
    [Key]
    public int RepairOrderId { get; set; }

    /// <summary>
    /// Id hóa đơn.
    /// </summary>
    [ForeignKey(nameof(Bill.Invoice))]
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
    /// Thông tin chủ xe (Navigation Property).
    /// </summary>
    public virtual Customer Owner { get; set; }

    /// <summary>
    /// Mã xe liên quan đến đơn sửa chữa.
    /// </summary>
    [ForeignKey(nameof(Vehicle))]
    public int CarId { get; set; }

    /// <summary>
    /// Thông tin xe liên quan (Navigation Property).
    /// </summary>
    public virtual Vehicle Car { get; set; }

    /// <summary>
    /// Ngày lập đơn.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public virtual List<RepairTask> RepairTaskList { get; set; } = [];

    /// <summary>
    /// Danh sách phụ tùng thay thế liên quan.
    /// </summary>
    public virtual List<ReplacementPart> ReplacementPartList { get; set; } = [];

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    public decimal TotalRepairCost() =>
        (RepairTaskList?.Sum(task => task.ServiceItem.UnitPrice) ?? 0) +
        (ReplacementPartList?.Sum(part => part.UnitPrice) ?? 0);
}