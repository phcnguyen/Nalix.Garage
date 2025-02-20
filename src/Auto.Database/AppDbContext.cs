using Auto.Common.Models.Bill;
using Auto.Common.Models.Customers;
using Auto.Common.Models.Employees;
using Auto.Common.Models.Part;
using Auto.Common.Models.Repair;
using Auto.Common.Models.Suppliers;
using Auto.Common.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Auto.Database;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }
    public DbSet<Car> Cars { get; set; }
    public DbSet<SparePart> SpareParts { get; set; }
    public DbSet<RepairOrder> RepairOrders { get; set; }
    public DbSet<RepairTask> RepairTasks { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}