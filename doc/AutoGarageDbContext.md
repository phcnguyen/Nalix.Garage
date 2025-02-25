# AutoGarageDbContext - Tài liệu chi tiết về Cơ sở dữ liệu Garage Ô tô

## 1. Tổng quan

`AutoGarageDbContext` là lớp kế thừa từ `DbContext` trong Entity Framework Core, đại diện cho cơ sở dữ liệu quản lý hoạt động của một garage ô tô. Nó bao gồm các bảng (entities) liên quan đến xe cộ, khách hàng, nhân viên, nhà cung cấp, phụ tùng, hóa đơn, giao dịch, và lịch sử sửa chữa. Thiết kế tập trung vào **hiệu suất**, **tính nhất quán dữ liệu**, và **khả năng mở rộng**.

- **Ngày tạo**: Dựa trên dữ liệu mẫu, hệ thống giả định hoạt động từ ngày **25/02/2025**.
- **Mục tiêu**: Quản lý toàn bộ quy trình từ tiếp nhận xe, lập hóa đơn, sửa chữa, đến theo dõi giao dịch và tồn kho phụ tùng.

---

## 2. Danh sách các bảng (Entities)

### 2.1. Vehicle (Cars)

- **Mô tả**: Đại diện cho xe ô tô của khách hàng.
- **Các cột chính**:
  - `CarId` (int, PK): Mã xe, khóa chính.
  - `OwnerId` (int, FK → Customer): Mã khách hàng sở hữu xe.
  - `CarLicensePlate` (string, max 9): Biển số xe, duy nhất.
  - `CarYear` (int): Năm sản xuất.
  - `Mileage` (double): Số km đã đi.
- **Quan hệ**:
  - 1-N với `Customer` (1 khách hàng sở hữu nhiều xe).
  - 1-N với `RepairHistory` (1 xe có nhiều lịch sử sửa chữa).
- **Chỉ mục (Indexes)**:
  - `CarLicensePlate` (Unique): Tối ưu tìm kiếm theo biển số.
  - `OwnerId`: Tăng tốc truy vấn xe theo khách hàng.
  - `CarBrand`: Tối ưu lọc theo hãng xe.

### 2.2. Invoice (Invoices)

- **Mô tả**: Đại diện cho hóa đơn phát hành cho khách hàng.
- **Các cột chính**:
  - `InvoiceId` (int, PK): Mã hóa đơn.
  - `OwnerId` (int, FK → Customer): Mã khách hàng.
  - `CreatedBy` (int, FK → Employee): Nhân viên tạo hóa đơn.
  - `InvoiceNumber` (string, max 30): Số hóa đơn, duy nhất.
  - `Discount` (decimal(18,2)): Giá trị giảm giá.
- **Quan hệ**:
  - 1-N với `Transaction` (1 hóa đơn có nhiều giao dịch).
  - 1-1 với `RepairOrder` (1 hóa đơn liên kết 1 đơn sửa chữa).
- **Chỉ mục**:
  - `InvoiceNumber` (Unique): Đảm bảo không trùng số hóa đơn.
  - `OwnerId`: Tối ưu truy vấn hóa đơn theo khách hàng.
  - `CreatedBy`: Tăng tốc truy vấn theo người tạo.
- **Query Filter**: Lọc mặc định các hóa đơn chưa thanh toán xong (`!IsFullyPaid`).

### 2.3. Customer (Customers)

- **Mô tả**: Đại diện cho khách hàng của garage.
- **Các cột chính**:
  - `CustomerId` (int, PK): Mã khách hàng.
  - `Email` (string, max 100): Email, duy nhất.
  - `PhoneNumber` (string, max 12): Số điện thoại, duy nhất.
  - `TaxCode` (string, max 13): Mã số thuế.
- **Quan hệ**:
  - 1-N với `Vehicle` (1 khách hàng có nhiều xe).
  - 1-N với `RepairOrder` (1 khách hàng có nhiều đơn sửa chữa).
- **Chỉ mục**:
  - `Email` (Unique): Đảm bảo không trùng email.
  - `PhoneNumber` (Unique): Đảm bảo không trùng số điện thoại.
  - `TaxCode`: Tối ưu tìm kiếm theo mã số thuế.

### 2.4. Employee (Employees)

- **Mô tả**: Đại diện cho nhân viên trong garage.
- **Các cột chính**:
  - `EmployeeId` (int, PK): Mã nhân viên.
  - `Email` (string, max 50): Email, duy nhất.
  - `PhoneNumber` (string, max 14): Số điện thoại.
  - `Status` (enum): Trạng thái làm việc.
- **Quan hệ**:
  - 1-N với `RepairTask` (1 nhân viên thực hiện nhiều công việc sửa chữa).
- **Chỉ mục**:
  - `Email` (Unique): Đảm bảo không trùng email.
  - `PhoneNumber`: Tăng tốc tìm kiếm theo số điện thoại.
  - `Status`: Tối ưu lọc nhân viên theo trạng thái.

### 2.5. Supplier (Suppliers)

- **Mô tả**: Đại diện cho nhà cung cấp phụ tùng.
- **Các cột chính**:
  - `SupplierId` (int, PK): Mã nhà cung cấp.
  - `Email` (string, max 100): Email, duy nhất.
  - `TaxCode` (string, max 13): Mã số thuế, duy nhất.
  - `Status` (enum): Trạng thái hợp tác.
- **Quan hệ**:
  - 1-N với `SparePart` (1 nhà cung cấp cung cấp nhiều phụ tùng).
  - 1-N với `SupplierPhone` (1 nhà cung cấp có nhiều số điện thoại).
- **Chỉ mục**:
  - `Email` (Unique): Đảm bảo không trùng email.
  - `TaxCode` (Unique): Đảm bảo không trùng mã số thuế.
  - `Status`: Tối ưu lọc theo trạng thái.

### 2.6. SupplierPhone

- **Mô tả**: Lưu thông tin số điện thoại của nhà cung cấp.
- **Các cột chính**:
  - `Id` (int, PK): Mã định danh.
  - `SupplierId` (int, FK → Supplier): Mã nhà cung cấp.
  - `PhoneNumber` (string, max 14): Số điện thoại.
- **Quan hệ**:
  - N-1 với `Supplier`.
- **Chỉ mục**:
  - `{SupplierId, PhoneNumber}`: Tối ưu truy vấn số điện thoại theo nhà cung cấp.

### 2.7. SparePart (SpareParts)

- **Mô tả**: Đại diện cho phụ tùng trong kho.
- **Các cột chính**:
  - `PartId` (int, PK): Mã phụ tùng.
  - `SupplierId` (int, FK → Supplier): Mã nhà cung cấp.
  - `PartName` (string, max 100): Tên phụ tùng.
  - `PurchasePrice` (decimal(18,2)): Giá nhập.
  - `SellingPrice` (decimal(18,2)): Giá bán.
- **Quan hệ**:
  - N-1 với `Supplier`.
- **Chỉ mục**:
  - `{SupplierId, PartName}` (Unique): Đảm bảo tên duy nhất trong cùng nhà cung cấp.
  - `PartName`: Tăng tốc tìm kiếm theo tên.

### 2.8. RepairTask (RepairTasks)

- **Mô tả**: Đại diện cho công việc sửa chữa cụ thể.
- **Các cột chính**:
  - `RepairTaskId` (int, PK): Mã công việc.
  - `EmployeeId` (int, FK → Employee): Mã nhân viên thực hiện.
  - `ServiceItemId` (int, FK → ServiceItem): Mã dịch vụ.
  - `Status` (enum): Trạng thái công việc.
- **Quan hệ**:
  - N-1 với `Employee`.
  - N-1 với `ServiceItem`.
  - 1-N với `RepairOrder`.
- **Chỉ mục**:
  - `Status`: Tối ưu lọc theo trạng thái.
  - `{StartDate, CompletionDate}`: Tối ưu truy vấn theo thời gian.
  - `EmployeeId`: Tối ưu truy vấn theo nhân viên.
- **Ràng buộc**:
  - `CompletionDate >= StartDate OR CompletionDate IS NULL`.

### 2.9. ServiceItem (ServiceItem)

- **Mô tả**: Đại diện cho dịch vụ sửa chữa/bảo dưỡng.
- **Các cột chính**:
  - `ServiceItemId` (int, PK): Mã dịch vụ.
  - `Description` (string, max 255): Mô tả dịch vụ.
  - `UnitPrice` (decimal(18,2)): Đơn giá.
- **Quan hệ**:
  - 1-N với `RepairTask`.
- **Chỉ mục**:
  - `Description`: Tăng tốc tìm kiếm theo mô tả.
  - `Type`: Tối ưu lọc theo loại dịch vụ.

### 2.10. RepairOrder (RepairOrders)

- **Mô tả**: Đại diện cho đơn sửa chữa.
- **Các cột chính**:
  - `RepairOrderId` (int, PK): Mã đơn sửa chữa.
  - `InvoiceId` (int, FK → Invoice): Mã hóa đơn.
  - `OwnerId` (int, FK → Customer): Mã khách hàng.
  - `CarId` (int, FK → Vehicle): Mã xe.
- **Quan hệ**:
  - 1-1 với `Invoice`.
  - N-1 với `Customer`.
  - N-1 với `Vehicle`.
  - 1-N với `RepairTask`.
  - 1-N với `ReplacementPart`.
- **Chỉ mục**:
  - `OwnerId`: Tối ưu truy vấn theo khách hàng.
  - `CarId`: Tối ưu truy vấn theo xe.
  - `InvoiceId`: Tối ưu truy vấn theo hóa đơn.

### 2.11. Transaction (Transactions)

- **Mô tả**: Đại diện cho giao dịch tài chính.
- **Các cột chính**:
  - `TransactionId` (int, PK): Mã giao dịch.
  - `InvoiceId` (int, FK → Invoice): Mã hóa đơn.
  - `Amount` (decimal): Số tiền.
  - `Type` (enum, int): Loại giao dịch.
- **Quan hệ**:
  - N-1 với `Invoice`.
- **Chỉ mục**:
  - `InvoiceId`: Tối ưu truy vấn theo hóa đơn.
  - `CreatedBy`: Tối ưu truy vấn theo người tạo.
  - `TransactionDate`: Tối ưu truy vấn theo thời gian.
  - `Status`: Tối ưu lọc theo trạng thái.
  - `Type`: Tối ưu lọc theo loại giao dịch.
- **Ràng buộc**:
  - `Amount > 0`.
  - `TransactionDate <= GETUTCDATE()`.

### 2.12. RepairHistory (RepairHistories)

- **Mô tả**: Đại diện cho lịch sử sửa chữa của xe.
- **Các cột chính**:
  - `HistoryId` (int, PK): Mã lịch sử.
  - `CarId` (int, FK → Vehicle): Mã xe.
  - `TotalCost` (decimal(18,2)): Tổng chi phí.
- **Quan hệ**:
  - N-1 với `Vehicle`.
  - 1-N với `RepairTask`.
- **Chỉ mục**:
  - `CarId`: Tối ưu truy vấn theo xe.
  - `RepairDate`: Tối ưu truy vấn theo ngày sửa chữa.

### 2.13. ReplacementPart (ReplacementPart)

- **Mô tả**: Đại diện cho phụ tùng thay thế.
- **Các cột chính**:
  - `PartId` (int, PK): Mã phụ tùng.
  - `PartCode` (string, max 12): Mã SKU, duy nhất.
  - `UnitPrice` (decimal(18,2)): Đơn giá.
- **Quan hệ**:
  - 1-N với `RepairOrder`.
- **Chỉ mục**:
  - `PartCode` (Unique): Đảm bảo mã duy nhất.
  - `Manufacturer`: Tối ưu lọc theo nhà sản xuất.

---

## 3. Các điểm tối ưu nổi bật

1. **Chỉ mục (Indexing)**:
   - Được thêm vào các cột thường xuyên truy vấn hoặc lọc (như `Email`, `PhoneNumber`, `Status`, `TransactionDate`).
   - Chỉ mục composite (ví dụ: `{SupplierId, PartName}`, `{StartDate, CompletionDate}`) để tối ưu truy vấn phức tạp.
2. **Ràng buộc (Constraints)**:
   - Đảm bảo dữ liệu hợp lệ (ví dụ: `Amount > 0`, `CompletionDate >= StartDate`).
   - Ngăn lỗi logic (ví dụ: `TransactionDate <= GETUTCDATE()`).
3. **Quan hệ**:
   - Hành vi xóa được cấu hình rõ ràng (`Cascade`, `Restrict`, `SetNull`, `NoAction`) để tránh mất dữ liệu không mong muốn.
4. **Hiệu suất**:
   - Precision (18,2) áp dụng cho các cột tiền tệ để tránh sai số.
   - Query Filter (`!IsFullyPaid`) giúp giảm tải truy vấn hóa đơn.
5. **Dữ liệu mẫu**:
   - `ReplacementPart` có dữ liệu khởi tạo để thử nghiệm (PartCode: "ABC123").

---

## 4. Gợi ý cải tiến trong tương lai

- **Partitioning**: Nếu bảng `Transaction` hoặc `RepairHistory` lớn, cân nhắc phân vùng theo `TransactionDate` hoặc `RepairDate`.
- **Caching**: Sử dụng cache (như Redis) cho các truy vấn tĩnh (ví dụ: danh sách `ServiceItem`).
- **Audit Trail**: Thêm bảng lưu lịch sử thay đổi (như `AuditLog`) để theo dõi chỉnh sửa.

---

## 5. Example Usage (Ví dụ sử dụng)

Dưới đây là các ví dụ chi tiết minh họa cách sử dụng `AutoGarageDbContext` trong các trường hợp thực tế, bao gồm thêm dữ liệu, truy vấn, và cập nhật.

### 5.1. Thiết lập DbContext

```csharp
using Microsoft.EntityFrameworkCore;
using Auto.Database;

var options = new DbContextOptionsBuilder<AutoGarageDbContext>()
    .UseSqlServer("Server=localhost;Database=AutoGarage;Trusted_Connection=True;")
    .Options;

using var context = new AutoGarageDbContext(options);
```

### 5.2. Thêm khách hàng và xe mới

```csharp
// Thêm khách hàng
var customer = new Customer
{
    Name = "Nguyen Van A",
    PhoneNumber = "0909123456",
    Email = "nguyenvana@example.com",
    Address = "123 Đường Láng, Hà Nội",
    Type = CustomerType.Individual
};
context.Customers.Add(customer);
context.SaveChanges();

// Thêm xe cho khách hàng
var vehicle = new Vehicle
{
    OwnerId = customer.CustomerId,
    CarLicensePlate = "29A-12345",
    CarYear = 2020,
    CarBrand = CarBrand.Toyota,
    CarModel = "Camry",
    Mileage = 15000
};
context.Cars.Add(vehicle);
context.SaveChanges();
```

### 5.3. Tạo hóa đơn và đơn sửa chữa

```csharp
// Tạo hóa đơn
var invoice = new Invoice
{
    OwnerId = customer.CustomerId,
    CreatedBy = 1, // Giả sử EmployeeId = 1
    InvoiceNumber = "INV-20250225-001",
    TaxRate = TaxRateType.VAT10,
    DiscountType = DiscountType.Percentage,
    Discount = 5m // Giảm 5%
};
context.Invoices.Add(invoice);

// Tạo đơn sửa chữa
var repairOrder = new RepairOrder
{
    InvoiceId = invoice.InvoiceId,
    OwnerId = customer.CustomerId,
    CarId = vehicle.CarId,
    RepairTaskList = new List<RepairTask>
    {
        new RepairTask
        {
            EmployeeId = 1,
            ServiceItemId = 1, // Giả sử có ServiceItemId = 1
            Status = RepairOrderStatus.Pending,
            StartDate = DateTime.UtcNow,
            EstimatedDuration = 2.5
        }
    }
};
context.RepairOrders.Add(repairOrder);
context.SaveChanges();
```

### 5.4. Thêm giao dịch thanh toán

```csharp
var transaction = new Transaction
{
    InvoiceId = invoice.InvoiceId,
    Type = TransactionType.Revenue,
    PaymentMethod = PaymentMethod.Cash,
    Status = TransactionStatus.Completed,
    Amount = 1000000m, // 1 triệu VND
    Description = "Thanh toán tiền sửa chữa",
    CreatedBy = 1
};
context.Transactions.Add(transaction);
context.SaveChanges();

Console.WriteLine($"Số tiền còn nợ: {invoice.BalanceDue}");
```

### 5.5. Truy vấn lịch sử sửa chữa của xe

```csharp
var carRepairHistory = context.RepairHistories
    .Where(rh => rh.CarId == vehicle.CarId)
    .Include(rh => rh.RepairTaskList)
        .ThenInclude(rt => rt.ServiceItem)
    .OrderBy(rh => rh.RepairDate)
    .ToList();

foreach (var history in carRepairHistory)
{
    Console.WriteLine($"Ngày sửa: {history.RepairDate}, Tổng chi phí: {history.TotalCost}");
    foreach (var task in history.RepairTaskList)
    {
        Console.WriteLine($"- Công việc: {task.ServiceItem.Description}, Trạng thái: {task.Status}");
    }
}
```

### 5.6. Cập nhật trạng thái công việc sửa chữa

```csharp
var repairTask = context.RepairTasks
    .FirstOrDefault(rt => rt.RepairTaskId == 1);

if (repairTask != null)
{
    repairTask.Status = RepairOrderStatus.Completed;
    repairTask.CompletionDate = DateTime.UtcNow;
    context.SaveChanges();
    Console.WriteLine("Công việc đã hoàn thành!");
}
```

### 5.7. Thêm phụ tùng và điều chỉnh tồn kho

```csharp
var supplier = context.Suppliers
    .FirstOrDefault(s => s.SupplierId == 1);

var sparePart = new SparePart
{
    SupplierId = supplier.SupplierId,
    PartName = "Lọc dầu Toyota",
    PurchasePrice = 100000m,
    SellingPrice = 150000m,
    InventoryQuantity = 10
};
context.SpareParts.Add(sparePart);
context.SaveChanges();

// Giảm tồn kho
sparePart.AdjustInventory(-2); // Sử dụng 2 cái
context.SaveChanges();
Console.WriteLine($"Số lượng tồn kho còn lại: {sparePart.InventoryQuantity}");
```

### 5.8. Truy vấn hóa đơn chưa thanh toán

```csharp
var unpaidInvoices = context.Invoices
    .Where(i => !i.IsFullyPaid)
    .Include(i => i.Owner)
    .ToList();

foreach (var inv in unpaidInvoices)
{
    Console.WriteLine($"Hóa đơn: {inv.InvoiceNumber}, Khách hàng: {inv.Owner.Name}, Còn nợ: {inv.BalanceDue}");
}
```

### 5.9. Truy vấn doanh thu theo khoảng thời gian

- Mục đích: Tính tổng doanh thu từ các giao dịch thanh toán trong một khoảng thời gian (ví dụ: tháng 2/2025).

```csharp
var startDate = new DateTime(2025, 2, 1);
var endDate = new DateTime(2025, 2, 28);

var revenue = context.Transactions
    .Where(t => t.Type == TransactionType.Revenue 
             && t.Status == TransactionStatus.Completed 
             && t.TransactionDate >= startDate 
             && t.TransactionDate <= endDate)
    .Sum(t => t.Amount);

Console.WriteLine($"Doanh thu từ {startDate:dd/MM/yyyy} đến {endDate:dd/MM/yyyy}: {revenue:N0} VND");
```

### 5.10. Kiểm tra tồn kho phụ tùng sắp hết

- Mục đích: Liệt kê các phụ tùng có số lượng tồn kho dưới ngưỡng tối thiểu (ví dụ: dưới 5 cái).

```csharp
var lowStockParts = context.SpareParts
    .Where(sp => sp.InventoryQuantity < 5 && !sp.IsDiscontinued)
    .Include(sp => sp.Supplier)
    .ToList();

foreach (var part in lowStockParts)
{
    Console.WriteLine($"Phụ tùng: {part.PartName}, Tồn kho: {part.InventoryQuantity}, Nhà cung cấp: {part.Supplier.Name}");
}
```

### 5.11. Tạo báo cáo công việc của nhân viên

- Mục đích: Lấy danh sách công việc sửa chữa mà một nhân viên đã thực hiện trong tuần qua.

```csharp
var employeeId = 1;
var lastWeek = DateTime.UtcNow.AddDays(-7);

var employeeTasks = context.RepairTasks
    .Where(rt => rt.EmployeeId == employeeId 
              && rt.StartDate >= lastWeek)
    .Include(rt => rt.ServiceItem)
    .OrderBy(rt => rt.StartDate)
    .ToList();

Console.WriteLine($"Công việc của nhân viên ID {employeeId} trong tuần qua:");
foreach (var task in employeeTasks)
{
    Console.WriteLine($"- {task.ServiceItem.Description}, Trạng thái: {task.Status}, Bắt đầu: {task.StartDate}");
}
```

### 5.12. Thêm nhà cung cấp và phụ tùng cùng lúc

- Mục đích: Thêm một nhà cung cấp mới cùng với danh sách phụ tùng và số điện thoại liên quan.

```csharp
var supplier = new Supplier
{
    Name = "Công ty Phụ tùng XYZ",
    Email = "xyz@parts.com",
    TaxCode = "1234567890",
    PhoneNumbers = new List<SupplierPhone>
    {
        new SupplierPhone { PhoneNumber = "0987654321" }
    },
    SpareParts = new List<SparePart>
    {
        new SparePart
        {
            PartName = "Lọc gió XYZ",
            PurchasePrice = 80000m,
            SellingPrice = 120000m,
            InventoryQuantity = 20
        }
    }
};
context.Suppliers.Add(supplier);
context.SaveChanges();

Console.WriteLine($"Đã thêm nhà cung cấp {supplier.Name} với mã ID {supplier.SupplierId}");
```

### 5.13. Tìm xe có lịch sử sửa chữa nhiều nhất

```csharp
var mostRepairedCar = context.Cars
    .Select(v => new
    {
        Vehicle = v,
        RepairCount = v.RepairHistoryes.Count
    })
    .OrderByDescending(x => x.RepairCount)
    .FirstOrDefault();

if (mostRepairedCar != null)
{
    Console.WriteLine($"Xe sửa nhiều nhất: {mostRepairedCar.Vehicle.CarLicensePlate}, Số lần sửa: {mostRepairedCar.RepairCount}");
}
```

### 5.14. Cập nhật giá dịch vụ hàng loạt

```csharp
var maintenanceServices = context.ServiceItem
    .Where(si => si.Type == ServiceType.Maintenance)
    .ToList();

foreach (var service in maintenanceServices)
{
    service.UnitPrice *= 1.10m; // Tăng 10%
}
context.SaveChanges();

Console.WriteLine($"Đã cập nhật giá cho {maintenanceServices.Count} dịch vụ bảo dưỡng.");
```

### 5.15. Lấy danh sách hóa đơn quá hạn thanh toán

```csharp
var overdueInvoices = context.Invoices
    .Where(i => !i.IsFullyPaid 
             && i.InvoiceDate < DateTime.UtcNow.AddDays(-7))
    .Include(i => i.Owner)
    .ToList();

foreach (var inv in overdueInvoices)
{
    Console.WriteLine($"Hóa đơn: {inv.InvoiceNumber}, Khách hàng: {inv.Owner.Name}, Quá hạn: {(DateTime.UtcNow - inv.InvoiceDate).Days} ngày");
}
```

### 5.16. Kiểm tra xe sắp hết hạn bảo hiểm

```csharp
var upcomingExpiries = context.Cars
    .Where(v => v.InsuranceExpiryDate.HasValue 
             && v.InsuranceExpiryDate.Value <= DateTime.UtcNow.AddDays(30)
             && v.InsuranceExpiryDate.Value >= DateTime.UtcNow)
    .Include(v => v.Owner)
    .ToList();

foreach (var car in upcomingExpiries)
{
    Console.WriteLine($"Xe: {car.CarLicensePlate}, Chủ xe: {car.Owner.Name}, Hết hạn bảo hiểm: {car.InsuranceExpiryDate.Value:dd/MM/yyyy}");
}
```

### 5.17. Tính tổng chi phí sửa chữa của khách hàng

```csharp
var customerId = 1;
var startOfYear = new DateTime(DateTime.UtcNow.Year, 1, 1);

var totalRepairCost = context.RepairOrders
    .Where(ro => ro.OwnerId == customerId 
              && ro.Invoice.InvoiceDate >= startOfYear)
    .Sum(ro => ro.Invoice.TotalAmount);

Console.WriteLine($"Tổng chi phí sửa chữa của khách hàng ID {customerId} trong năm {DateTime.UtcNow.Year}: {totalRepairCost:N0} VND");
```

### 5.18. Thêm phụ tùng thay thế vào đơn sửa chữa

```csharp
var repairOrder = context.RepairOrders
    .Include(ro => ro.ReplacementPartList)
    .FirstOrDefault(ro => ro.RepairOrderId == 1);

var sparePart = context.SpareParts
    .FirstOrDefault(sp => sp.PartId == 1);

if (repairOrder != null && sparePart != null)
{
    var replacementPart = new ReplacementPart
    {
        PartCode = "REP-001",
        PartName = sparePart.PartName,
        UnitPrice = sparePart.SellingPrice,
        Quantity = 1,
        Manufacturer = "OEM"
    };
    repairOrder.ReplacementPartList.Add(replacementPart);
    sparePart.AdjustInventory(-1); // Giảm tồn kho
    context.SaveChanges();
    Console.WriteLine($"Đã thêm phụ tùng {replacementPart.PartName} vào đơn sửa chữa ID {repairOrder.RepairOrderId}");
}
```

### 5.19. Tạo báo cáo phụ tùng bán chạy

```csharp
var topParts = context.ReplacementPart
    .GroupBy(rp => new { rp.PartCode, rp.PartName })
    .Select(g => new
    {
        PartCode = g.Key.PartCode,
        PartName = g.Key.PartName,
        TotalUsed = g.Sum(rp => rp.Quantity)
    })
    .OrderByDescending(x => x.TotalUsed)
    .Take(5)
    .ToList();

Console.WriteLine("Top 5 phụ tùng bán chạy:");
foreach (var part in topParts)
{
    Console.WriteLine($"- {part.PartName} (Mã: {part.PartCode}), Số lượng: {part.TotalUsed}");
}
```

### 5.20. Kiểm tra nhà cung cấp ngừng hợp tác

```csharp
var inactiveSuppliersWithStock = context.Suppliers
    .Where(s => s.Status == SupplierStatus.Inactive)
    .Include(s => s.SpareParts)
    .Where(s => s.SpareParts.Any(sp => sp.InventoryQuantity > 0))
    .ToList();

foreach (var supplier in inactiveSuppliersWithStock)
{
    Console.WriteLine($"Nhà cung cấp: {supplier.Name}, Còn tồn kho: {supplier.SpareParts.Sum(sp => sp.InventoryQuantity)} phụ tùng");
}
```

### 5.21. Xuất danh sách công việc chưa hoàn thành

```csharp
var pendingTasks = context.RepairTasks
    .Where(rt => rt.Status == RepairOrderStatus.Pending || rt.Status == RepairOrderStatus.InProgress)
    .Include(rt => rt.Employee)
    .Include(rt => rt.ServiceItem)
    .ToList();

Console.WriteLine("Danh sách công việc chưa hoàn thành:");
foreach (var task in pendingTasks)
{
    Console.WriteLine($"- Dịch vụ: {task.ServiceItem.Description}, Nhân viên: {task.Employee.Name}, Trạng thái: {task.Status}");
}
```

### 5.22. Gửi thông báo hóa đơn quá hạn qua email giả lập

```csharp
var overdueInvoices = context.Invoices
    .Where(i => !i.IsFullyPaid && i.InvoiceDate < DateTime.UtcNow.AddDays(-7))
    .Include(i => i.Owner)
    .ToList();

foreach (var invoice in overdueInvoices)
{
    var daysOverdue = (DateTime.UtcNow - invoice.InvoiceDate).Days;
    var message = $"Kính gửi {invoice.Owner.Name},\n" +
                  $"Hóa đơn {invoice.InvoiceNumber} của bạn đã quá hạn {daysOverdue} ngày. " +
                  $"Số tiền còn lại: {invoice.BalanceDue:N0} VND. Vui lòng thanh toán sớm.\n" +
                  $"Trân trọng,\nGarage Ô tô XYZ";
    Console.WriteLine($"Gửi email đến {invoice.Owner.Email}:\n{message}\n---");
}
```

## 6. Kết luận

`AutoGarageDbContext` được thiết kế với mục tiêu tối ưu hiệu suất và đảm bảo tính toàn vẹn dữ liệu. Các chỉ mục và ràng buộc được triển khai cẩn thận để hỗ trợ truy vấn nhanh và tránh lỗi logic. Các ví dụ trên minh họa cách sử dụng thực tế, từ thêm dữ liệu, truy vấn, đến cập nhật trạng thái. Đây là nền tảng vững chắc cho ứng dụng quản lý garage ô tô, có thể mở rộng khi nhu cầu tăng.
