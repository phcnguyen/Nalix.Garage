using Auto.Common.Entities.Authentication;
using Auto.Common.Entities.Bill;
using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Employees;
using Auto.Common.Entities.Part;
using Auto.Common.Entities.Repair;
using Auto.Common.Entities.Service;
using Auto.Common.Entities.Suppliers;
using Auto.Common.Entities.Transactions;
using Auto.Common.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;
using System;

namespace Auto.Database;

/// <summary>
/// DbContext cho ứng dụng quản lý gara ô tô.
/// </summary>
public class AutoDbContext(DbContextOptions<AutoDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Cars { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierPhone> SupplierPhones { get; set; }
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<RepairTask> RepairTasks { get; set; }
    public DbSet<ServiceItem> ServiceItems { get; set; }
    public DbSet<RepairOrder> RepairOrders { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<RepairHistory> RepairHistories { get; set; }
    public DbSet<ReplacementPart> ReplacementParts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        ConfigureAccount(modelBuilder);
        ConfigureVehicle(modelBuilder);
        ConfigureInvoice(modelBuilder);
        ConfigureCustomer(modelBuilder);
        ConfigureEmployee(modelBuilder);
        ConfigureSupplier(modelBuilder);
        ConfigureSparePart(modelBuilder);
        ConfigureRepairTask(modelBuilder);
        ConfigureServiceItem(modelBuilder);
        ConfigureRepairOrder(modelBuilder);
        ConfigureTransaction(modelBuilder);
        ConfigureRepairHistory(modelBuilder);
        ConfigureReplacementPart(modelBuilder);
    }

    private static void ConfigureAccount(ModelBuilder modelBuilder)
    {
        // Đảm bảo Username là duy nhất để tránh trùng lặp tài khoản trong hệ thống
        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Username)
            .IsUnique();

        // Chuyển đổi thuộc tính Role (kiểu enum) thành byte khi lưu vào cơ sở dữ liệu
        // Giúp giảm kích thước cột, tối ưu hiệu suất lưu trữ và truy vấn
        modelBuilder.Entity<Account>()
            .Property(a => a.Role)
            .HasConversion<byte>();
    }

    private static void ConfigureVehicle(ModelBuilder modelBuilder)
    {
        // Chuyển đổi enum thành byte để tiết kiệm dung lượng và tối ưu hiệu suất truy vấn
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.CarBrand)
            .HasConversion<byte>();

        modelBuilder.Entity<Vehicle>()
            .Property(v => v.CarType)
            .HasConversion<byte>();

        modelBuilder.Entity<Vehicle>()
            .Property(v => v.CarColor)
            .HasConversion<byte>();

        // Đảm bảo mỗi biển số xe là duy nhất để tránh trùng lặp dữ liệu
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.CarLicensePlate)
            .IsUnique();

        // Index cho CustomerId giúp tối ưu truy vấn khi tìm kiếm xe theo chủ sở hữu
        modelBuilder.Entity<Vehicle>().HasIndex(v => v.CustomerId);

        // Index cho CarBrand giúp tăng tốc tìm kiếm xe theo hãng
        modelBuilder.Entity<Vehicle>().HasIndex(v => v.CarBrand);

        // Index phức hợp giúp tối ưu truy vấn khi lọc theo nhiều tiêu chí
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => new { v.CarBrand, v.CarType, v.CarColor, v.CarYear });
    }

    private static void ConfigureInvoice(ModelBuilder modelBuilder)
    {
        // Định dạng số thập phân cho Discount với tối đa 18 chữ số và 2 số lẻ
        modelBuilder.Entity<Invoice>()
            .Property(i => i.Discount)
            .HasPrecision(18, 2);

        // Đảm bảo mã hóa đơn là duy nhất để tránh trùng lặp
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.InvoiceNumber)
            .IsUnique();

        // Index
        // 1.Giúp tối ưu truy vấn theo chủ hóa đơn
        // 2.Hỗ trợ truy vấn nhanh theo người tạo hóa đơn
        // 3.Giúp tối ưu tìm kiếm hóa đơn theo ngày lập
        modelBuilder.Entity<Invoice>().HasIndex(i => i.OwnerId);
        modelBuilder.Entity<Invoice>().HasIndex(i => i.CreatedBy);
        modelBuilder.Entity<Invoice>().HasIndex(i => i.InvoiceDate);

        // Bộ lọc toàn cục (Global Query Filter) để chỉ lấy hóa đơn chưa thanh toán
        // modelBuilder.Entity<Invoice>().HasQueryFilter(i => !i.IsFullyPaid);
    }

    private static void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        // Đảm bảo email khách hàng là duy nhất
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        // Đảm bảo số điện thoại khách hàng là duy nhất
        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.PhoneNumber)
            .IsUnique();

        // Index hỗ trợ truy vấn theo mã số thuế và theo tên
        modelBuilder.Entity<Customer>().HasIndex(c => c.TaxCode);
        modelBuilder.Entity<Customer>().HasIndex(c => c.Name);
    }

    private static void ConfigureEmployee(ModelBuilder modelBuilder)
    {
        // Đảm bảo email nhân viên là duy nhất
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();

        // Index giúp tối ưu truy vấn theo số điện thoại và trạng thái làm việc
        modelBuilder.Entity<Employee>().HasIndex(e => e.PhoneNumber);
        modelBuilder.Entity<Employee>().HasIndex(e => e.Status);

        // Chuyển đổi enum thành byte để tối ưu lưu trữ
        modelBuilder.Entity<Employee>()
            .Property(e => e.Gender)
            .HasConversion<byte>();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Position)
            .HasConversion<byte>();

        modelBuilder.Entity<Employee>()
            .Property(e => e.Status)
            .HasConversion<byte>();
    }

    private static void ConfigureSupplier(ModelBuilder modelBuilder)
    {
        // Đảm bảo email và mã số thuế của nhà cung cấp là duy nhất
        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.Email)
            .IsUnique();

        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.TaxCode)
            .IsUnique();

        // Index giúp tối ưu truy vấn theo trạng thái nhà cung cấp
        modelBuilder.Entity<Supplier>()
            .HasIndex(s => s.Status);

        // Quan hệ 1-N giữa Supplier và PhoneNumbers
        modelBuilder.Entity<Supplier>()
            .HasMany(s => s.PhoneNumbers)
            .WithOne(sp => sp.Supplier)
            .HasForeignKey(sp => sp.SupplierId)
            .OnDelete(DeleteBehavior.Cascade); // Xóa Supplier sẽ xóa luôn danh sách số điện thoại

        // Tạo index cho (SupplierId, PhoneNumber) giúp tối ưu tìm kiếm
        modelBuilder.Entity<SupplierPhone>().HasIndex(sp => new { sp.SupplierId, sp.PhoneNumber });
    }

    private static void ConfigureSparePart(ModelBuilder modelBuilder)
    {
        // Cấu hình độ chính xác cho giá mua và giá bán
        modelBuilder.Entity<SparePart>()
            .Property(sp => sp.PurchasePrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<SparePart>()
            .Property(sp => sp.SellingPrice)
            .HasPrecision(18, 2);

        // Quan hệ 1-N giữa Supplier và SparePart
        modelBuilder.Entity<SparePart>()
            .HasOne(sp => sp.Supplier)
            .WithMany(s => s.SpareParts)
            .HasForeignKey(sp => sp.SupplierId)
            .OnDelete(DeleteBehavior.Restrict); // Không cho phép xóa Supplier nếu còn SparePart

        // Đảm bảo tên phụ tùng không trùng trong cùng một nhà cung cấp
        modelBuilder.Entity<SparePart>()
            .HasIndex(sp => new { sp.SupplierId, sp.PartName })
            .IsUnique();

        // Index giúp tăng tốc tìm kiếm phụ tùng theo tên
        modelBuilder.Entity<SparePart>().HasIndex(sp => sp.PartName);
    }

    private static void ConfigureRepairTask(ModelBuilder modelBuilder)
    {
        // Quan hệ 1-N với Employee
        modelBuilder.Entity<RepairTask>()
            .HasOne(rt => rt.Employee)
            .WithMany()
            .HasForeignKey(rt => rt.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Quan hệ 1-N với ServiceItem
        modelBuilder.Entity<RepairTask>()
            .HasOne(rt => rt.ServiceItem)
            .WithMany()
            .HasForeignKey(rt => rt.ServiceItemId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index tối ưu tìm kiếm và truy vấn
        modelBuilder.Entity<RepairTask>().HasIndex(rt => rt.Status);
        modelBuilder.Entity<RepairTask>().HasIndex(rt => new { rt.StartDate, rt.CompletionDate });
        modelBuilder.Entity<RepairTask>().HasIndex(rt => rt.EmployeeId);

        // Ràng buộc kiểm tra CompletionDate phải lớn hơn hoặc bằng StartDate hoặc null
        modelBuilder.Entity<RepairTask>()
            .ToTable(tb => tb.HasCheckConstraint("CK_RepairTask_CompletionDate", "CompletionDate >= StartDate OR CompletionDate IS NULL"));
    }

    private static void ConfigureServiceItem(ModelBuilder modelBuilder)
    {
        // Index giúp tối ưu tìm kiếm theo mô tả dịch vụ và loại dịch vụ
        modelBuilder.Entity<ServiceItem>().HasIndex(si => si.Description);
        modelBuilder.Entity<ServiceItem>().HasIndex(si => si.Type);

        modelBuilder.Entity<ServiceItem>()
            .Property(si => si.UnitPrice)
            .HasPrecision(18, 2);
    }

    private static void ConfigureRepairOrder(ModelBuilder modelBuilder)
    {
        // Thiết lập quan hệ và ràng buộc xóa
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

        // Tạo index tối ưu truy vấn
        modelBuilder.Entity<RepairOrder>().HasIndex(ro => ro.OwnerId);
        modelBuilder.Entity<RepairOrder>().HasIndex(ro => ro.CarId);
        modelBuilder.Entity<RepairOrder>().HasIndex(ro => ro.InvoiceId);
    }

    private static void ConfigureTransaction(ModelBuilder modelBuilder)
    {
        // Chuyển đổi enum Type thành byte để lưu trữ hiệu quả hơn
        modelBuilder.Entity<Transaction>()
            .Property(t => t.Type)
            .HasConversion<byte>();

        // Thiết lập quan hệ với Invoice
        modelBuilder.Entity<Transaction>()
            .HasOne<Invoice>()
            .WithMany()
            .HasForeignKey(t => t.InvoiceId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa Transaction nếu Invoice bị xóa

        // Tạo index để tối ưu truy vấn
        modelBuilder.Entity<Transaction>().HasIndex(t => t.Status);
        modelBuilder.Entity<Transaction>().HasIndex(t => t.Type);
        modelBuilder.Entity<Transaction>().HasIndex(t => t.InvoiceId);
        modelBuilder.Entity<Transaction>().HasIndex(t => t.CreatedBy);
        modelBuilder.Entity<Transaction>().HasIndex(t => t.TransactionDate);

        // Ràng buộc dữ liệu để đảm bảo tính hợp lệ
        modelBuilder.Entity<Transaction>()
            .ToTable(tb => tb.HasCheckConstraint("CK_Transaction_Amount", "Amount > 0"));

        modelBuilder.Entity<Transaction>()
            .ToTable(tb => tb.HasCheckConstraint("CK_Transaction_Date", "TransactionDate <= GETUTCDATE()"));
    }

    private static void ConfigureRepairHistory(ModelBuilder modelBuilder)
    {
        // Định dạng kiểu dữ liệu chính xác cho TotalCost
        modelBuilder.Entity<RepairHistory>()
            .Property(rh => rh.TotalCost)
            .HasPrecision(18, 2);

        // Thiết lập quan hệ với Vehicle
        modelBuilder.Entity<RepairHistory>()
            .HasOne(rh => rh.Vehicle)
            .WithMany(v => v.RepairHistories)
            .HasForeignKey(rh => rh.VehicleId)
            .OnDelete(DeleteBehavior.Restrict); // Không xóa RepairHistory nếu Vehicle bị xóa

        // Tạo index để tối ưu truy vấn
        modelBuilder.Entity<RepairHistory>().HasIndex(rh => rh.VehicleId);
        modelBuilder.Entity<RepairHistory>().HasIndex(rh => rh.RepairDate);
    }

    private static void ConfigureReplacementPart(ModelBuilder modelBuilder)
    {
        // Đảm bảo mã phụ tùng là duy nhất để tránh trùng lặp dữ liệu
        modelBuilder.Entity<ReplacementPart>()
            .HasIndex(rp => rp.PartCode)
            .IsUnique();

        // Index hỗ trợ tìm kiếm nhanh theo nhà sản xuất
        modelBuilder.Entity<ReplacementPart>().HasIndex(rp => rp.Manufacturer);

        // Định dạng kiểu dữ liệu chính xác cho giá phụ tùng
        modelBuilder.Entity<ReplacementPart>()
            .Property(rp => rp.UnitPrice)
            .HasPrecision(18, 2);

        // Thêm dữ liệu mẫu
        modelBuilder.Entity<ReplacementPart>().HasData(
            new ReplacementPart
            {
                PartId = 1,
                PartCode = "ABC123",
                PartName = "Brake Pad",
                DateAdded = new DateOnly(2025, 2, 25),
                ExpiryDate = new DateOnly(2026, 2, 25),
                UnitPrice = 150.50m,
                Manufacturer = "OEM"
            });
    }
}