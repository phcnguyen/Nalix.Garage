# Garage Management System

## Overview

This document describes the functionalities, data, and requirements of the **Garage Management System**. The system includes several modules to manage customers, vehicles, repair orders, employees, inventory, finances, reports, and security.

---

## A. Customer Management

### Main Features

- Add/Edit/Delete customer information.
- Search and filter customers by name, phone number, and license plate.
- View repair history of each customer.
- Track customer debts.

### Data to Store

- Customer ID
- Full Name
- Phone Number
- Address
- List of vehicles belonging to the customer
- Repair history
- Outstanding debts

---

## B. Vehicle Management

### Main Features

- Add/Edit/Delete vehicle information.
- Manage vehicle details (license plate, type, color, chassis number, engine number, etc.).
- Search vehicles by license plate.
- View repair history.
- Track periodic maintenance.

### Data to Store

- Vehicle ID
- License Plate
- Vehicle Brand
- Model
- Color
- Chassis Number
- Engine Number
- Vehicle Owner (linked to customer)
- Repair and maintenance history

---

## C. Repair Order Management

### Main Features

- Create repair orders (select customer, vehicle, list of tasks, spare parts).
- Update order status:
  - Awaiting Confirmation (Pre-order by customer or created by staff)
  - In Progress (Repair in process)
  - Completed (Repair finished, awaiting payment)
  - Paid (Order completed)
- Print repair invoices.

### Data to Store

- Order ID
- Order Date
- Status
- Customer
- Vehicle
- List of repair tasks (description, unit price, staff performing)
- List of spare parts (name, quantity, unit price)
- Total repair cost

---

## D. Employee Management

### Main Features

- Add/Edit/Delete employee information.
- Manage employee details (name, birthdate, position, phone number).
- Track employee tasks (assign repair tasks, monitor work performance).
- Calculate employee salary (if performance-based).

### Data to Store

- Employee ID
- Full Name
- Birthdate
- Phone Number
- Position
- List of completed tasks
- Salary details (if applicable)

---

## E. Spare Parts Inventory Management

### Main Features

- Add/Edit/Delete spare parts.
- Monitor stock (alert when low quantity).
- Add parts from suppliers.
- Deduct parts when used in repairs.
- Manage spare part suppliers.

### Data to Store

- Spare Part ID
- Name
- Part Type
- Unit Price
- Stock Quantity
- Supplier (name, address, phone number)

---

## F. Financial Management

### Main Features

- Manage invoices (collect payments from customers, pay for spare parts).
- Track revenue by day, month, and year.
- Profit report (Revenue - Expenses).
- Monitor customer debts.

### Data to Store

- Invoice ID
- Invoice Date
- Customer
- Total Amount
- Payment Status
- List of income/expenses

---

## G. Reports & Statistics

### Main Features

- Revenue reports over time.
- Report on the number of vehicles repaired.
- Spare parts inventory report.
- Employee performance report.

### Data to Display

- Revenue Charts
- Repair Statistics Table (by day, month, year)
- Spare Parts Inventory Movement (in/out)
- Employee Performance Metrics

---

## H. System & Security

### Main Features

- User role management (Admin, Receptionist, Accountant, Technician).
- Manage logins, password changes.
- Data backup and recovery.

### Data to Store

- User Roles
- Login Credentials
- Backup Information
