using Microsoft.EntityFrameworkCore;
using NalixGarage.Common.Entities.Authentication;
using NalixGarage.Common.Entities.Bill;
using NalixGarage.Common.Entities.Customers;
using NalixGarage.Common.Entities.Employees;
using NalixGarage.Common.Entities.Part;
using NalixGarage.Common.Entities.Repair;
using NalixGarage.Common.Entities.Service;
using NalixGarage.Common.Entities.Suppliers;
using NalixGarage.Common.Entities.Transactions;
using NalixGarage.Common.Entities.Vehicles;

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