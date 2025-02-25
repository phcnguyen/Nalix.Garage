using Auto.Common.Entities;
using Auto.Common.Entities.Bill;
using Auto.Common.Entities.Bill.Transactions;
using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Employees;
using Auto.Common.Entities.Part;
using Auto.Common.Entities.Repair;
using Auto.Common.Entities.Service;
using Auto.Common.Entities.Suppliers;
using Auto.Common.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;
using System;

namespace Auto.Database;

public class AutoDbContext(DbContextOptions<AutoDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Cars { get; set; }
    public DbSet<Account> Account { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierPhone> SupplierPhone { get; set; }
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<RepairTask> RepairTasks { get; set; }
    public DbSet<ServiceItem> ServiceItem { get; set; }
    public DbSet<RepairOrder> RepairOrders { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<RepairHistory> RepairHistories { get; set; }
    public DbSet<ReplacementPart> ReplacementPart { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        #region Account Configuration

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Username)
            .IsUnique(); // Đảm bảo username duy nhất

        modelBuilder.Entity<Account>()
            .Property(a => a.Role)
            .HasConversion<byte>(); // Chuyển đổi enum RoleType thành int trong DB

        #endregion Account Configuration

        #region Vehicle Configuration

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Username)
            .IsUnique(); // Đảm bảo username duy nhất

        modelBuilder.Entity<Account>()
            .Property(a => a.Role)
            .HasConversion<byte>(); // Chuyển đổi enum RoleType thành int trong DB

        #endregion Vehicle Configuration

        #region Vehicle Configuration

        // Cấu hình cho bảng Vehicle
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.CarLicensePlate)
            .IsUnique(); // Tối ưu tìm kiếm theo biển số xe và đảm bảo duy nhất

        // Đặt index cho các cột thường được tìm kiếm
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.OwnerId);

        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.CarBrand);

        // Index phức hợp cho tìm kiếm phức tạp
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => new { v.CarBrand, v.CarType, v.CarYear });

        #endregion Vehicle Configuration

        #region Invoice Configuration

        modelBuilder.Entity<Invoice>()
            .Property(i => i.Discount)
            .HasPrecision(18, 2);

        // Giới hạn duy nhất cho mã hóa đơn
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique(); // Đảm bảo mã hóa đơn không trùng lặp

        // Index cho truy vấn thường xuyên
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.OwnerId);

        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.CreatedBy);

        // Index cho tìm kiếm theo ngày
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.InvoiceDate);

        // Global query filter cho hóa đơn chưa thanh toán
        modelBuilder.Entity<Invoice>()
            .HasQueryFilter(i => !i.IsFullyPaid);

        #endregion Invoice Configuration

        #region Customer Configuration

        // Cấu hình cho bảng Customer
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique(); // Đảm bảo email khách hàng duy nhất

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique(); // Đảm bảo số điện thoại khách hàng duy nhất

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.TaxCode);

        // Thêm index cho tìm kiếm theo tên khách hàng
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Name);

        #endregion Customer Configuration

        #region Employee Configuration

        // Cấu hình cho bảng Employee
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique(); // Đảm bảo email nhân viên không trùng

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.PhoneNumber); // Tăng tốc tìm kiếm theo số điện thoại

        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Status);

        #endregion Employee Configuration

        #region Supplier Configuration

        // Cấu hình cho bảng Supplier
        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.Email)
            .IsUnique(); // Đảm bảo email nhà cung cấp duy nhất

        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.TaxCode)
            .IsUnique(); // Đảm bảo mã số thuế nhà cung cấp không trùng

        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.Status);

        // Quan hệ 1-N giữa Supplier và PhoneNumbers
        modelBuilder.Entity<Supplier>()
            .HasMany(s => s.PhoneNumbers)
            .WithOne(sp => sp.Supplier)
            .HasForeignKey(sp => sp.SupplierId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa Supplier sẽ xóa luôn danh sách số điện thoại

        modelBuilder.Entity<SupplierPhone>()
            .HasIndex(sp => new { sp.SupplierId, sp.PhoneNumber });

        #endregion Supplier Configuration

        #region SparePart Configuration

        // Cấu hình cho bảng SparePart
        modelBuilder.Entity<SparePart>()
            .HasOne(sp => sp.Supplier)
            .WithMany(s => s.SpareParts)
            .HasForeignKey(sp => sp.SupplierId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa SparePart nếu Supplier bị xóa

        modelBuilder.Entity<SparePart>()
            .HasIndex(sp => new { sp.SupplierId, sp.PartName })
            .IsUnique(); // Đảm bảo tên phụ tùng không trùng trong cùng nhà cung cấp

        modelBuilder.Entity<SparePart>()
            .HasIndex(sp => sp.PartName); // Tăng tốc tìm kiếm phụ tùng theo tên

        modelBuilder.Entity<SparePart>()
            .Property(sp => sp.PurchasePrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SparePart>()
            .Property(sp => sp.SellingPrice)
            .HasPrecision(18, 2);

        #endregion SparePart Configuration

        #region RepairTask Configuration

        // Cấu hình cho bảng RepairTask
        modelBuilder.Entity<RepairTask>()
            .HasOne(rt => rt.Employee)
            .WithMany()
            .HasForeignKey(rt => rt.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa RepairTask nếu Employee bị xóa

        modelBuilder.Entity<RepairTask>()
            .HasOne(rt => rt.ServiceItem)
            .WithMany()
            .HasForeignKey(rt => rt.ServiceItemId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa RepairTask nếu ServiceItem bị xóa

        modelBuilder.Entity<RepairTask>()
            .HasIndex(rt => rt.Status); // Tối ưu tìm kiếm theo trạng thái công việc

        modelBuilder.Entity<RepairTask>()
            .HasIndex(rt => new { rt.StartDate, rt.CompletionDate }); // Tối ưu truy vấn theo thời gian

        modelBuilder.Entity<RepairTask>()
            .HasIndex(rt => rt.EmployeeId);

        modelBuilder.Entity<RepairTask>()
            .HasAnnotation("CheckConstraint", "CompletionDate >= StartDate OR CompletionDate IS NULL");

        #endregion RepairTask Configuration

        #region ServiceItem Configuration

        // Cấu hình cho bảng ServiceItem
        modelBuilder.Entity<ServiceItem>()
            .HasIndex(si => si.Description); // Tăng tốc tìm kiếm theo mô tả dịch vụ

        modelBuilder.Entity<ServiceItem>()
            .HasIndex(si => si.Type);

        #endregion ServiceItem Configuration

        #region RepairOrder Configuration

        // Cấu hình cho bảng RepairOrder
        modelBuilder.Entity<RepairOrder>()
            .HasOne(ro => ro.Invoice)
            .WithMany()
            .HasForeignKey(ro => ro.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa Invoice nếu có RepairOrder liên quan

        modelBuilder.Entity<RepairOrder>()
            .HasOne(ro => ro.Owner)
            .WithMany()
            .HasForeignKey(ro => ro.OwnerId)
            .OnDelete(DeleteBehavior.NoAction); // Giữ RepairOrder nếu Customer bị xóa

        modelBuilder.Entity<RepairOrder>()
            .HasOne(ro => ro.Vehicle)
            .WithMany()
            .HasForeignKey(ro => ro.CarId)
            .OnDelete(DeleteBehavior.SetNull); // Set VehicleId về null nếu Vehicle bị xóa

        modelBuilder.Entity<RepairOrder>()
            .HasMany(ro => ro.RepairTaskList)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade); // Xóa RepairOrder sẽ xóa luôn RepairTask

        modelBuilder.Entity<RepairOrder>()
            .HasMany(ro => ro.ReplacementPartList)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade); // Xóa RepairOrder sẽ xóa luôn ReplacementPart

        // Tối ưu truy vấn RepairOrder
        modelBuilder.Entity<RepairOrder>()
            .HasIndex(ro => ro.OwnerId);

        modelBuilder.Entity<RepairOrder>()
            .HasIndex(ro => ro.CarId);

        modelBuilder.Entity<RepairOrder>()
            .HasIndex(ro => ro.InvoiceId);

        #endregion RepairOrder Configuration

        #region Transaction Configuration

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Type)
            .HasConversion<byte>();

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.Status);

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.Type);

        // Cấu hình cho bảng Transaction
        modelBuilder.Entity<Transaction>()
            .HasOne<Invoice>()
            .WithMany()
            .HasForeignKey(t => t.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa Transaction nếu Invoice bị xóa

        // Tối ưu truy vấn Transaction
        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.InvoiceId);

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.CreatedBy);

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.TransactionDate);

        // Ràng buộc dữ liệu
        modelBuilder.Entity<Transaction>()
            .HasAnnotation("CheckConstraint", "Amount > 0"); // Đảm bảo số tiền giao dịch lớn hơn 0

        modelBuilder.Entity<Transaction>()
            .HasAnnotation("CheckConstraint", "TransactionDate <= GETUTCDATE()");

        #endregion Transaction Configuration

        #region RepairHistory Configuration

        // Cấu hình cho bảng RepairHistory
        modelBuilder.Entity<RepairHistory>()
            .HasOne(rh => rh.Vehicle)
            .WithMany(v => v.RepairHistoryes)
            .HasForeignKey(rh => rh.VehicleId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa Vehicle nếu có RepairHistory liên quan

        modelBuilder.Entity<RepairHistory>()
            .HasIndex(rh => rh.VehicleId); // Tối ưu truy vấn theo VehicleId

        modelBuilder.Entity<RepairHistory>()
            .HasIndex(rh => rh.RepairDate);

        modelBuilder.Entity<RepairHistory>()
            .Property(rh => rh.TotalCost)
            .HasPrecision(18, 2);

        #endregion RepairHistory Configuration

        #region ReplacementPart Configuration

        // Cấu hình cho bảng ReplacementPart
        modelBuilder.Entity<ReplacementPart>()
            .HasIndex(rp => rp.PartCode)
            .IsUnique(); // Đảm bảo mã phụ tùng duy nhất và tối ưu tìm kiếm

        // Đảm bảo ngày hết hạn không nhỏ hơn ngày thêm vào (dữ liệu mẫu)
        modelBuilder.Entity<ReplacementPart>()
            .HasData(new ReplacementPart
            {
                PartId = 1,
                PartCode = "ABC123",
                PartName = "Brake Pad",
                DateAdded = new DateOnly(2025, 2, 25),
                ExpiryDate = new DateOnly(2026, 2, 25),
                UnitPrice = 150.50m,
                Manufacturer = "OEM"
            });

        modelBuilder.Entity<ReplacementPart>()
            .HasIndex(rp => rp.Manufacturer);

        // Định nghĩa kiểu dữ liệu cho UnitPrice
        modelBuilder.Entity<ReplacementPart>()
            .Property(rp => rp.UnitPrice)
            .HasPrecision(18, 2); // 18 chữ số, 2 số thập phân

        #endregion ReplacementPart Configuration
    }
}