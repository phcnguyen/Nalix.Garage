using Auto.Common.Entites.Bill;
using Auto.Common.Entites.Customers;
using Auto.Common.Entites.Employees;
using Auto.Common.Entites.Part;
using Auto.Common.Entites.Repair;
using Auto.Common.Entites.Suppliers;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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
    public DbSet<RepairOrder> RepairOrders { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasIndex(c => c.Email).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.PhoneNumber).IsUnique();
    }
}