using Auto.Common.Models.Part;
using System;
using System.Collections.Generic;

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
    /// Ngày lập đơn.
    /// </summary>
    public DateTime OrderDate { get; set; }

    /// <summary>
    /// Trạng thái của đơn sửa chữa.
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// Khách hàng liên quan đến đơn sửa chữa.
    /// </summary>
    public Customer Customer { get; set; }

    /// <summary>
    /// Xe liên quan đến đơn sửa chữa.
    /// </summary>
    public Car Car { get; set; }

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public List<RepairTask> RepairTaskList { get; set; }

    /// <summary>
    /// Danh sách phụ tùng thay thế liên quan.
    /// </summary>
    public List<ReplacementPart> ReplacementPartList { get; set; }

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    public decimal TotalRepairCost { get; set; }
}