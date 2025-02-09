```text
Models/
├── Car.cs                            # Định nghĩa lớp xe
├── Customer.cs                       # Định nghĩa lớp khách hàng
├── Employee.cs                       # Định nghĩa lớp nhân viên
├── Invoice.cs                        # Định nghĩa lớp hóa đơn
├── Supplier.cs                       # Định nghĩa lớp nhà cung cấp
├── Transaction.cs                    # Định nghĩa lớp giao dịch
│
├── Part/                             # Chứa các đối tượng liên quan phụ tùng
│   ├── SparePart.cs                  # Định nghĩa lớp phụ tùng
│   └── ReplacementPart.cs            # Định nghĩa lớp phụ tùng thay thế
│
├── Repair/                           # Chứa các đối tượng liên quan đến sửa chữa
│   ├── RepairHistory.cs              # Lịch sử sửa chữa
│   ├── RepairOrder.cs                # Đơn sửa chữa
│   └── RepairTask.cs                 # Công việc sửa chữa
│
└── Report/                           # Chứa các báo cáo
    ├── CarRepairReport.cs            # Báo cáo sửa chữa xe
    ├── EmployeePerformanceReport.cs  # Báo cáo hiệu suất nhân viên
    ├── RevenueReport.cs              # Báo cáo doanh thu
    └── SparePartInventoryReport.cs   # Báo cáo tồn kho phụ tùng
```