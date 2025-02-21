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

namespace Auto.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Vehicle> Cars { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<RepairTask> RepairTasks { get; set; }
    public DbSet<ServiceItem> ServiceItem { get; set; }
    public DbSet<RepairOrder> RepairOrders { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<RepairHistory> RepairHistories { get; set; }
    public DbSet<ReplacementPart> ReplacementPart { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.PhoneNumber).IsUnique();
    }
}