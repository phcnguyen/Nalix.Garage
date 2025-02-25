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

## 6. Kết luận

`AutoGarageDbContext` được thiết kế với mục tiêu tối ưu hiệu suất và đảm bảo tính toàn vẹn dữ liệu. Các chỉ mục và ràng buộc được triển khai cẩn thận để hỗ trợ truy vấn nhanh và tránh lỗi logic. Các ví dụ trên minh họa cách sử dụng thực tế, từ thêm dữ liệu, truy vấn, đến cập nhật trạng thái. Đây là nền tảng vững chắc cho ứng dụng quản lý garage ô tô, có thể mở rộng khi nhu cầu tăng.
