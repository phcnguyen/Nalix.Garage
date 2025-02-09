using Auto.Common.Models.Cars;
using Auto.Common.Models.Part;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auto.Common.Models.Repair;

/// <summary>
/// Lớp đại diện cho đơn sửa chữa.
/// </summary>
public class RepairOrder
{
    /// <summary>
    /// Mã đơn sửa chữa.
    /// </summary>
    public int RepairOrderId { get; set; }

    /// <summary>
    /// Id chủ xe.
    /// </summary>
    public int OwnerId { get; set; }

    /// <summary>
    /// Ngày lập đơn.
    /// </summary>
    public DateTime OrderDate { get; set; } = DateTime.Now;

    /// <summary>
    /// Trạng thái của đơn sửa chữa.
    /// </summary>
    public RepairOrderStatus Status { get; set; } = RepairOrderStatus.Pending;

    /// <summary>
    /// Xe liên quan đến đơn sửa chữa.
    /// </summary>
    public Car Car { get; set; }

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public virtual List<RepairTask> RepairTaskList { get; set; }

    /// <summary>
    /// Danh sách phụ tùng thay thế liên quan.
    /// </summary>
    public virtual List<ReplacementPart> ReplacementPartList { get; set; }

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Selling price must be greater than zero.")]
    public decimal TotalRepairCost { get; set; }
}