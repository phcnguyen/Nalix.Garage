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

namespace Auto.Database;

public interface IAutoDbContext
{
    DbSet<Vehicle> Vehicles { get; set; }
    DbSet<Account> Accounts { get; set; }
    DbSet<Invoice> Invoices { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<Supplier> Suppliers { get; set; }
    DbSet<SparePart> SpareParts { get; set; }
    DbSet<RepairTask> RepairTasks { get; set; }
    DbSet<ServiceItem> ServiceItems { get; set; }
    DbSet<RepairOrder> RepairOrders { get; set; }
    DbSet<Transaction> Transactions { get; set; }
    DbSet<SupplierPhone> SupplierPhones { get; set; }
    DbSet<ReplacementPart> ReplacementParts { get; set; }
    DbSet<RepairOrderSparePart> RepairOrderSpareParts { get; set; }

    int SaveChanges();
}
