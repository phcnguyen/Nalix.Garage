using Auto.Common.Entities.Bill;
using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Employees;
using Auto.Common.Entities.Part;
using Auto.Common.Entities.Repair;
using Auto.Common.Entities.Service;
using Auto.Common.Entities.Suppliers;
using Auto.Common.Entities.Vehicles;
using Auto.Common.Models.Cars;
using Auto.Common.Models.Payments;
using Auto.Database;
using Auto.Database.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auto
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new AppDbContext(options);
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

        private static async Task TestCustomerRepository(AppDbContext context)
        {
            var customerRepository = new Repository<Customer>(context);

            // Test Add Customer
            var newCustomer = new Customer { Name = "John Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
            await customerRepository.AddAsync(newCustomer);
            Console.WriteLine("Added new customer.");

            // Test Get Customer
            var customer = await customerRepository.GetByIdAsync(newCustomer.CustomerId);
            Console.WriteLine($"Retrieved customer: {customer.Name}, {customer.Email}, {customer.PhoneNumber}");

            // Test Update Customer
            customer.PhoneNumber = "0987654321";
            customerRepository.Update(customer);
            Console.WriteLine("Updated customer phone number.");

            // Test Delete Customer
            await customerRepository.DeleteAsync(customer.CustomerId);
            Console.WriteLine("Deleted customer.");

            // Save changes
            await customerRepository.SaveChangesAsync();
        }

        private static async Task TestVehicleRepository(AppDbContext context)
        {
            var vehicleRepository = new Repository<Vehicle>(context);

            // Test Add Vehicle
            var newVehicle = new Vehicle { OwnerId = 1, CarYear = 2020, CarType = CarType.Sedan, CarColor = CarColor.None, CarBrand = CarBrand.None, CarLicensePlate = "ABC123", CarModel = "Camry", FrameNumber = "FR12345", EngineNumber = "EN12345" };
            await vehicleRepository.AddAsync(newVehicle);
            Console.WriteLine("Added new vehicle.");

            // Test Get Vehicle
            var vehicle = await vehicleRepository.GetByIdAsync(newVehicle.CarId);
            Console.WriteLine($"Retrieved vehicle: {vehicle.CarModel}, {vehicle.CarYear}");

            // Test Update Vehicle
            vehicle.CarYear = 2021;
            vehicleRepository.Update(vehicle);
            Console.WriteLine("Updated vehicle year.");

            // Test Delete Vehicle
            await vehicleRepository.DeleteAsync(vehicle.CarId);
            Console.WriteLine("Deleted vehicle.");

            // Save changes
            await vehicleRepository.SaveChangesAsync();
        }

        private static async Task TestEmployeeRepository(AppDbContext context)
        {
            var employeeRepository = new Repository<Employee>(context);

            // Test Add Employee
            var newEmployee = new Employee { Name = "Jane Smith", Gender = Gender.Female, DateOfBirth = new DateTime(1990, 1, 1), Address = "123 Street", PhoneNumber = "1234567890", Email = "jane.smith@example.com", Position = Position.Accountant, StartDate = DateTime.UtcNow, Status = EmploymentStatus.Active };
            await employeeRepository.AddAsync(newEmployee);
            Console.WriteLine("Added new employee.");

            // Test Get Employee
            var employee = await employeeRepository.GetByIdAsync(newEmployee.EmployeeId);
            Console.WriteLine($"Retrieved employee: {employee.Name}, {employee.Position}");

            // Test Update Employee
            employee.PhoneNumber = "0987654321";
            employeeRepository.Update(employee);
            Console.WriteLine("Updated employee phone number.");

            // Test Delete Employee
            await employeeRepository.DeleteAsync(employee.EmployeeId);
            Console.WriteLine("Deleted employee.");

            // Save changes
            await employeeRepository.SaveChangesAsync();
        }

        private static async Task TestInvoiceRepository(AppDbContext context)
        {
            var invoiceRepository = new Repository<Invoice>(context);

            // Test Add Invoice
            var newInvoice = new Invoice { OwnerId = 1, CreatedBy = 1, InvoiceNumber = "INV12345", InvoiceDate = DateTime.UtcNow, TaxRate = TaxRateType.VAT10, DiscountType = DiscountType.None, PaymentStatus = PaymentStatus.Unpaid, Discount = 0 };
            newInvoice.UpdateTotals();
            await invoiceRepository.AddAsync(newInvoice);
            Console.WriteLine("Added new invoice.");

            // Test Get Invoice
            var invoice = await invoiceRepository.GetByIdAsync(newInvoice.InvoiceId);
            Console.WriteLine($"Retrieved invoice: {invoice.InvoiceNumber}, {invoice.TotalAmount}");

            // Test Update Invoice
            invoice.Discount = 100;
            invoice.UpdateTotals();
            invoiceRepository.Update(invoice);
            Console.WriteLine("Updated invoice discount.");

            // Test Delete Invoice
            await invoiceRepository.DeleteAsync(invoice.InvoiceId);
            Console.WriteLine("Deleted invoice.");

            // Save changes
            await invoiceRepository.SaveChangesAsync();
        }

        private static async Task TestSparePartRepository(AppDbContext context)
        {
            var sparePartRepository = new Repository<SparePart>(context);

            // Test Add SparePart
            var newSparePart = new SparePart { SupplierId = 1, PartCategory = PartCategory.Brake, PartName = "Brake Pad", PurchasePrice = 20, SellingPrice = 25, IsDiscontinued = false };
            await sparePartRepository.AddAsync(newSparePart);
            Console.WriteLine("Added new spare part.");

            // Test Get SparePart
            var sparePart = await sparePartRepository.GetByIdAsync(newSparePart.PartId);
            Console.WriteLine($"Retrieved spare part: {sparePart.PartName}, {sparePart.SellingPrice}");

            // Test Update SparePart
            sparePart.SellingPrice = 30;
            sparePartRepository.Update(sparePart);
            Console.WriteLine("Updated spare part price.");

            // Test Delete SparePart
            await sparePartRepository.DeleteAsync(sparePart.PartId);
            Console.WriteLine("Deleted spare part.");

            // Save changes
            await sparePartRepository.SaveChangesAsync();
        }

        private static async Task TestRepairTaskRepository(AppDbContext context)
        {
            var repairTaskRepository = new Repository<RepairTask>(context);

            // Test Add RepairTask
            var newRepairTask = new RepairTask { EmployeeId = 1, ServiceItemId = 1, Status = RepairOrderStatus.Pending, StartDate = DateTime.UtcNow, EstimatedDuration = 2.5 };
            await repairTaskRepository.AddAsync(newRepairTask);
            Console.WriteLine("Added new repair task.");

            // Test Get RepairTask
            var repairTask = await repairTaskRepository.GetByIdAsync(newRepairTask.RepairTaskId);
            Console.WriteLine($"Retrieved repair task: {repairTask.ServiceItemId}, {repairTask.Status}");

            // Test Update RepairTask
            repairTask.Status = RepairOrderStatus.Completed;
            repairTask.CompletionDate = DateTime.UtcNow.AddHours(3);
            repairTaskRepository.Update(repairTask);
            Console.WriteLine("Updated repair task status.");

            // Test Delete RepairTask
            await repairTaskRepository.DeleteAsync(repairTask.RepairTaskId);
            Console.WriteLine("Deleted repair task.");

            // Save changes
            await repairTaskRepository.SaveChangesAsync();
        }

        private static async Task TestServiceItemRepository(AppDbContext context)
        {
            var serviceItemRepository = new Repository<ServiceItem>(context);

            // Test Add ServiceItem
            var newServiceItem = new ServiceItem { Description = "Tire Rotation", Type = ServiceType.Maintenance, UnitPrice = 25 };
            await serviceItemRepository.AddAsync(newServiceItem);
            Console.WriteLine("Added new service item.");

            // Test Get ServiceItem
            var serviceItem = await serviceItemRepository.GetByIdAsync(newServiceItem.ServiceId);
            Console.WriteLine($"Retrieved service item: {serviceItem.Description}, {serviceItem.UnitPrice}");

            // Test Update ServiceItem
            serviceItem.UnitPrice = 30;
            serviceItemRepository.Update(serviceItem);
            Console.WriteLine("Updated service item price.");

            // Test Delete ServiceItem
            await serviceItemRepository.DeleteAsync(serviceItem.ServiceId);
            Console.WriteLine("Deleted service item.");

            // Save changes
            await serviceItemRepository.SaveChangesAsync();
        }

        private static async Task TestSupplierRepository(AppDbContext context)
        {
            var supplierRepository = new Repository<Supplier>(context);

            // Test Add Supplier
            var newSupplier = new Supplier { Name = "Auto Parts Co.", Email = "contact@autoparts.com", Address = "123 Supplier Street", PhoneNumbers = ["1234567890"], Notes = "Reliable supplier", ContractStartDate = DateTime.UtcNow, BankAccount = "123456789", TaxCode = "TAX123", Status = SupplierStatus.Active, PaymentTerms = PaymentTerms.Net30 };
            await supplierRepository.AddAsync(newSupplier);
            Console.WriteLine("Added new supplier.");

            // Test Get Supplier
            var supplier = await supplierRepository.GetByIdAsync(newSupplier.SupplierId);
            Console.WriteLine($"Retrieved supplier: {supplier.Name}, {supplier.Email}");

            // Test Update Supplier
            supplier.Email = "support@autoparts.com";
            supplierRepository.Update(supplier);
            Console.WriteLine("Updated supplier email.");

            // Test Delete Supplier
            await supplierRepository.DeleteAsync(supplier.SupplierId);
            Console.WriteLine("Deleted supplier.");

            // Save changes
            await supplierRepository.SaveChangesAsync();
        }
    }
}