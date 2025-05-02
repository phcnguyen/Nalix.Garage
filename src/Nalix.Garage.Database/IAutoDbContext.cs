using Microsoft.EntityFrameworkCore;
using Nalix.Garage.Common.Entities.Authentication;
using Nalix.Garage.Common.Entities.Bill;
using Nalix.Garage.Common.Entities.Customers;
using Nalix.Garage.Common.Entities.Employees;
using Nalix.Garage.Common.Entities.Part;
using Nalix.Garage.Common.Entities.Repair;
using Nalix.Garage.Common.Entities.Service;
using Nalix.Garage.Common.Entities.Suppliers;
using Nalix.Garage.Common.Entities.Transactions;
using Nalix.Garage.Common.Entities.Vehicles;

namespace Nalix.Garage.Database;

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