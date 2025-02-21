# Hướng dẫn sử dụng các thực thể trong thư mục Auto.Common với Entity Framework Core

## Giới thiệu

Dự án Auto là một giải pháp phần mềm quản lý garage xe hơi, bao gồm các thực thể như khách hàng, xe, nhân viên, hóa đơn, phụ tùng, công việc sửa chữa, dịch vụ và nhà cung cấp. Hướng dẫn này sẽ giúp bạn sử dụng các thực thể này với Entity Framework Core để thao tác với cơ sở dữ liệu.

## Yêu cầu

- .NET 9.0 trở lên
- Entity Framework Core
- SQL Server hoặc cơ sở dữ liệu tương thích

## Cấu trúc thư mục

- `Auto.Common.Entities.Customers`: Chứa thực thể `Customer`.
- `Auto.Common.Entities.Vehicles`: Chứa thực thể `Vehicle`.
- `Auto.Common.Entities.Employees`: Chứa thực thể `Employee`.
- `Auto.Common.Entities.Bill`: Chứa thực thể `Invoice`.
- `Auto.Common.Entities.Part`: Chứa thực thể `SparePart`.
- `Auto.Common.Entities.Repair`: Chứa thực thể `RepairTask`.
- `Auto.Common.Entities.Service`: Chứa thực thể `ServiceItem`.
- `Auto.Common.Entities.Suppliers`: Chứa thực thể `Supplier`.

## Ví dụ sử dụng

### 1. Cài đặt package

Đầu tiên, cài đặt các package cần thiết:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

### 2. Cấu hình DbContext

Tạo lớp `AppDbContext` kế thừa từ `DbContext` và cấu hình các thực thể:

```csharp name=AppDbContext.cs
using Microsoft.EntityFrameworkCore;
using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Vehicles;
using Auto.Common.Entities.Employees;
using Auto.Common.Entities.Bill;
using Auto.Common.Entities.Part;
using Auto.Common.Entities.Repair;
using Auto.Common.Entities.Service;
using Auto.Common.Entities.Suppliers;

namespace Auto.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<SparePart> SpareParts { get; set; }
        public DbSet<RepairTask> RepairTasks { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
```

### 3. Cấu hình chuỗi kết nối

Thêm chuỗi kết nối vào file `appsettings.json`:

```json name=appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AutoDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 4. Khởi tạo DbContext trong `Program.cs`

```csharp name=Program.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Auto.Database;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
    })
    .Build();

await host.RunAsync();
```

### 5. Ví dụ CRUD cho các thực thể

#### Thêm mới một khách hàng

```csharp name=CustomerExample.cs
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Auto.Database;
using Auto.Common.Entities.Customers;

namespace CustomerExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("YourConnectionStringHere")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Thêm mới khách hàng
                var newCustomer = new Customer { Name = "John Doe", Email = "john.doe@example.com", PhoneNumber = "1234567890" };
                await context.Customers.AddAsync(newCustomer);
                await context.SaveChangesAsync();

                // Lấy thông tin khách hàng
                var customer = await context.Customers.FirstOrDefaultAsync(c => c.CustomerId == newCustomer.CustomerId);
                Console.WriteLine($"Retrieved customer: {customer.Name}, {customer.Email}, {customer.PhoneNumber}");

                // Cập nhật thông tin khách hàng
                customer.PhoneNumber = "0987654321";
                context.Customers.Update(customer);
                await context.SaveChangesAsync();
                Console.WriteLine("Updated customer phone number.");

                // Xóa khách hàng
                context.Customers.Remove(customer);
                await context.SaveChangesAsync();
                Console.WriteLine("Deleted customer.");
            }
        }
    }
}
```

#### Thêm mới một xe

```csharp name=VehicleExample.cs
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Auto.Database;
using Auto.Common.Entities.Vehicles;

namespace VehicleExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("YourConnectionStringHere")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Thêm mới xe
                var newVehicle = new Vehicle { OwnerId = 1, CarYear = 2020, CarType = CarType.Sedan, CarColor = CarColor.None, CarBrand = CarBrand.None, CarLicensePlate = "ABC123", CarModel = "Camry", FrameNumber = "FR12345", EngineNumber = "EN12345" };
                await context.Vehicles.AddAsync(newVehicle);
                await context.SaveChangesAsync();

                // Lấy thông tin xe
                var vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.CarId == newVehicle.CarId);
                Console.WriteLine($"Retrieved vehicle: {vehicle.CarModel}, {vehicle.CarYear}");

                // Cập nhật thông tin xe
                vehicle.CarYear = 2021;
                context.Vehicles.Update(vehicle);
                await context.SaveChangesAsync();
                Console.WriteLine("Updated vehicle year.");

                // Xóa xe
                context.Vehicles.Remove(vehicle);
                await context.SaveChangesAsync();
                Console.WriteLine("Deleted vehicle.");
            }
        }
    }
}
```

#### Thêm mới một nhân viên

```csharp name=EmployeeExample.cs
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Auto.Database;
using Auto.Common.Entities.Employees;

namespace EmployeeExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("YourConnectionStringHere")
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Thêm mới nhân viên
                var newEmployee = new Employee { Name = "Jane Smith", Gender = Gender.Female, DateOfBirth = new DateTime(1990, 1, 1), Address = "123 Street", PhoneNumber = "1234567890", Email = "jane.smith@example.com", Position = Position.Accountant, StartDate = DateTime.UtcNow, Status = EmploymentStatus.Active };
                await context.Employees.AddAsync(newEmployee);
                await context.SaveChangesAsync();

                // Lấy thông tin nhân viên
                var employee = await context.Employees.FirstOrDefaultAsync(e => e.EmployeeId == newEmployee.EmployeeId);
                Console.WriteLine($"Retrieved employee: {employee.Name}, {employee.Position}");

                // Cập nhật thông tin nhân viên
                employee.PhoneNumber = "0987654321";
                context.Employees.Update(employee);
                await context.SaveChangesAsync();
                Console.WriteLine("Updated employee phone number.");

                // Xóa nhân viên
                context.Employees.Remove(employee);
                await context.SaveChangesAsync();
                Console.WriteLine("Deleted employee.");
            }
        }
    }
}
```

## Kết luận

Hướng dẫn này cung cấp các ví dụ cơ bản về cách sử dụng Entity Framework Core để thao tác với các thực thể trong dự án Auto. Bạn có thể mở rộng các ví dụ này để phù hợp với nhu cầu cụ thể của mình.
