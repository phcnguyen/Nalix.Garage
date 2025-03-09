using Auto.Common.Entities.Vehicles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Auto.Common.Entities.Repair;

/// <summary>
/// Lớp đại diện cho lịch sử sửa chữa.
/// </summary>
[Table(nameof(RepairHistory))]
public class RepairHistory
{
    #region Fields

    // Hiện tại không có private fields, nhưng để lại region này cho tính nhất quán.

    #endregion

    #region Identification Properties

    /// <summary>
    /// Mã lịch sử sửa chữa.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Mã xe liên quan đến lịch sử sửa chữa.
    /// </summary>
    [ForeignKey(nameof(Vehicle))]
    public int VehicleId { get; set; }

    /// <summary>
    /// Thông tin xe liên quan (Navigation Property).
    /// </summary>
    public virtual Vehicle Vehicle { get; set; }

    #endregion

    #region Repair Details Properties

    /// <summary>
    /// Ngày sửa chữa.
    /// </summary>
    public DateTime RepairDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Danh sách công việc sửa chữa liên quan.
    /// </summary>
    public virtual ICollection<RepairTask> RepairTaskList { get; set; } = [];

    /// <summary>
    /// Tổng chi phí sửa chữa.
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalCost => (RepairTaskList ?? []).Sum(task => task.ServiceItem?.UnitPrice ?? 0);

    #endregion
}