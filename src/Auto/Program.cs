using Auto.Common.Entities.Bill;
using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Employees;
using Auto.Common.Entities.Part;
using Auto.Common.Entities.Repair;
using Auto.Common.Entities.Service;
using Auto.Common.Entities.Suppliers;
using Auto.Common.Entities.Vehicles;
using Auto.Database;
using Auto.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DatabaseTestApp
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("YourConnectionStringHere")
                .Options;

            using (var context = new AppDbContext(options))
            {
                try
                {
                    await TestCustomerRepository(context);
                    await TestVehicleRepository(context);
                    await TestEmployeeRepository(context);
                    await TestInvoiceRepository(context);
                    await TestSparePartRepository(context);
                    await TestRepairTaskRepository(context);
                    await TestServiceItemRepository(context);
                    await TestSupplierRepository(context);

                    Console.WriteLine("All tests completed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        private static async Task TestCustomerRepository(AppDbContext context)
        {
            var customerRepository = new Repository<Customer>(context);

            // Test Add Customer
            var newCustomer = new Customer { Name = "John Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
            await customerRepository.AddAsync(newCustomer);
            Console.WriteLine("Added new customer.");

            // Test Get Customer
            var customer = await customerRepository.GetByIdAsync(newCustomer.Id);
            Console.WriteLine($"Retrieved customer: {customer.Name}, {customer.Email}, {customer.PhoneNumber}");

            // Test Update Customer
            customer.PhoneNumber = "0987654321";
            customerRepository.Update(customer);
            Console.WriteLine("Updated customer phone number.");

            // Test Delete Customer
            await customerRepository.DeleteAsync(customer.Id);
            Console.WriteLine("Deleted customer.");

            // Save changes
            await customerRepository.SaveChangesAsync();
        }

        private static async Task TestVehicleRepository(AppDbContext context)
        {
            var vehicleRepository = new Repository<Vehicle>(context);

            // Test Add Vehicle
            var newVehicle = new Vehicle { Make = "Toyota", Model = "Camry", Year = 2020 };
            await vehicleRepository.AddAsync(newVehicle);
            Console.WriteLine("Added new vehicle.");

            // Test Get Vehicle
            var vehicle = await vehicleRepository.GetByIdAsync(newVehicle.Id);
            Console.WriteLine($"Retrieved vehicle: {vehicle.Make}, {vehicle.Model}, {vehicle.Year}");

            // Test Update Vehicle
            vehicle.Year = 2021;
            vehicleRepository.Update(vehicle);
            Console.WriteLine("Updated vehicle year.");

            // Test Delete Vehicle
            await vehicleRepository.DeleteAsync(vehicle.Id);
            Console.WriteLine("Deleted vehicle.");

            // Save changes
            await vehicleRepository.SaveChangesAsync();
        }

        private static async Task TestEmployeeRepository(AppDbContext context)
        {
            var employeeRepository = new Repository<Employee>(context);

            // Test Add Employee
            var newEmployee = new Employee { Name = "Jane Smith", Position = "Mechanic", Salary = 50000 };
            await employeeRepository.AddAsync(newEmployee);
            Console.WriteLine("Added new employee.");

            // Test Get Employee
            var employee = await employeeRepository.GetByIdAsync(newEmployee.Id);
            Console.WriteLine($"Retrieved employee: {employee.Name}, {employee.Position}, {employee.Salary}");

            // Test Update Employee
            employee.Salary = 55000;
            employeeRepository.Update(employee);
            Console.WriteLine("Updated employee salary.");

            // Test Delete Employee
            await employeeRepository.DeleteAsync(employee.Id);
            Console.WriteLine("Deleted employee.");

            // Save changes
            await employeeRepository.SaveChangesAsync();
        }

        private static async Task TestInvoiceRepository(AppDbContext context)
        {
            var invoiceRepository = new Repository<Invoice>(context);

            // Test Add Invoice
            var newInvoice = new Invoice { Amount = 1000, Date = DateTime.Now };
            await invoiceRepository.AddAsync(newInvoice);
            Console.WriteLine("Added new invoice.");

            // Test Get Invoice
            var invoice = await invoiceRepository.GetByIdAsync(newInvoice.Id);
            Console.WriteLine($"Retrieved invoice: {invoice.Amount}, {invoice.Date}");

            // Test Update Invoice
            invoice.Amount = 1200;
            invoiceRepository.Update(invoice);
            Console.WriteLine("Updated invoice amount.");

            // Test Delete Invoice
            await invoiceRepository.DeleteAsync(invoice.Id);
            Console.WriteLine("Deleted invoice.");

            // Save changes
            await invoiceRepository.SaveChangesAsync();
        }

        private static async Task TestSparePartRepository(AppDbContext context)
        {
            var sparePartRepository = new Repository<SparePart>(context);

            // Test Add SparePart
            var newSparePart = new SparePart { Name = "Brake Pad", Price = 50 };
            await sparePartRepository.AddAsync(newSparePart);
            Console.WriteLine("Added new spare part.");

            // Test Get SparePart
            var sparePart = await sparePartRepository.GetByIdAsync(newSparePart.Id);
            Console.WriteLine($"Retrieved spare part: {sparePart.Name}, {sparePart.Price}");

            // Test Update SparePart
            sparePart.Price = 55;
            sparePartRepository.Update(sparePart);
            Console.WriteLine("Updated spare part price.");

            // Test Delete SparePart
            await sparePartRepository.DeleteAsync(sparePart.Id);
            Console.WriteLine("Deleted spare part.");

            // Save changes
            await sparePartRepository.SaveChangesAsync();
        }

        private static async Task TestRepairTaskRepository(AppDbContext context)
        {
            var repairTaskRepository = new Repository<RepairTask>(context);

            // Test Add RepairTask
            var newRepairTask = new RepairTask { Description = "Oil Change", Cost = 30 };
            await repairTaskRepository.AddAsync(newRepairTask);
            Console.WriteLine("Added new repair task.");

            // Test Get RepairTask
            var repairTask = await repairTaskRepository.GetByIdAsync(newRepairTask.Id);
            Console.WriteLine($"Retrieved repair task: {repairTask.Description}, {repairTask.Cost}");

            // Test Update RepairTask
            repairTask.Cost = 35;
            repairTaskRepository.Update(repairTask);
            Console.WriteLine("Updated repair task cost.");

            // Test Delete RepairTask
            await repairTaskRepository.DeleteAsync(repairTask.Id);
            Console.WriteLine("Deleted repair task.");

            // Save changes
            await repairTaskRepository.SaveChangesAsync();
        }

        private static async Task TestServiceItemRepository(AppDbContext context)
        {
            var serviceItemRepository = new Repository<ServiceItem>(context);

            // Test Add ServiceItem
            var newServiceItem = new ServiceItem { Name = "Tire Rotation", Price = 25 };
            await serviceItemRepository.AddAsync(newServiceItem);
            Console.WriteLine("Added new service item.");

            // Test Get ServiceItem
            var serviceItem = await serviceItemRepository.GetByIdAsync(newServiceItem.Id);
            Console.WriteLine($"Retrieved service item: {serviceItem.Name}, {serviceItem.Price}");

            // Test Update ServiceItem
            serviceItem.Price = 30;
            serviceItemRepository.Update(serviceItem);
            Console.WriteLine("Updated service item price.");

            // Test Delete ServiceItem
            await serviceItemRepository.DeleteAsync(serviceItem.Id);
            Console.WriteLine("Deleted service item.");

            // Save changes
            await serviceItemRepository.SaveChangesAsync();
        }

        private static async Task TestSupplierRepository(AppDbContext context)
        {
            var supplierRepository = new Repository<Supplier>(context);

            // Test Add Supplier
            var newSupplier = new Supplier { Name = "Auto Parts Co.", ContactInfo = "contact@autoparts.com" };
            await supplierRepository.AddAsync(newSupplier);
            Console.WriteLine("Added new supplier.");

            // Test Get Supplier
            var supplier = await supplierRepository.GetByIdAsync(newSupplier.Id);
            Console.WriteLine($"Retrieved supplier: {supplier.Name}, {supplier.ContactInfo}");

            // Test Update Supplier
            supplier.ContactInfo = "support@autoparts.com";
            supplierRepository.Update(supplier);
            Console.WriteLine("Updated supplier contact info.");

            // Test Delete Supplier
            await supplierRepository.DeleteAsync(supplier.Id);
            Console.WriteLine("Deleted supplier.");

            // Save changes
            await supplierRepository.SaveChangesAsync();
        }
    }
}