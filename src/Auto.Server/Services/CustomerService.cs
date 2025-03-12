using Auto.Common.Entities.Customers;
using Auto.Common.Enums;
using Auto.Database;
using Microsoft.EntityFrameworkCore;
using Notio.Common.Attributes;
using Notio.Common.Connection;
using Notio.Common.Enums;
using Notio.Common.Interfaces;
using Notio.Network.Package;
using Notio.Network.Package.Enums;
using Notio.Network.Package.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Auto.Server.Services;

/// <summary>
/// Dịch vụ xử lý thông tin khách hàng.
/// </summary>
[PacketController]
public sealed class CustomerService(AutoDbContext context) : Base.BaseService
{
    private readonly AutoDbContext _context = context;

    /// <summary>
    /// Thêm khách hàng mới vào cơ sở dữ liệu.
    /// </summary>
    [PacketCommand((int)Command.AddCustomer, Authoritys.User)]
    public void AddCustomer(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 6, out string[] parts))
        {
            connection.Send(CreateErrorPacket("Invalid data format."));
            return;
        }

        // Chuẩn hóa dữ liệu
        string name = parts[0].Trim();
        string phone = parts[1].Trim();
        string email = parts[2].Trim();
        string address = parts[3].Trim();
        string taxCode = parts[4].Trim();
        CustomerType type = ParseEnum(parts[5], CustomerType.Individual);

        // Kiểm tra khách hàng đã tồn tại chưa
        if (_context.Customers.Any(c => c.PhoneNumber == phone || c.Email == email))
        {
            connection.Send(CreateErrorPacket("Customer with this phone or email already exists."));
            return;
        }

        // Tạo khách hàng mới
        try
        {
            Customer customer = new()
            {
                Name = name,
                PhoneNumber = phone,
                Email = email,
                Address = address,
                TaxCode = taxCode,
                Type = type
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            connection.Send(CreateSuccessPacket("Customer added successfully."));
        }
        catch (Exception ex)
        {
            connection.Send(CreateErrorPacket(ex.Message));
            return;
        }
    }

    /// <summary>
    /// Cập nhật thông tin khách hàng dựa trên ID.
    /// </summary>
    [PacketCommand((int)Command.UpdateCustomer, Authoritys.User)]
    public void UpdateCustomer(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 6, out string[] parts) || !int.TryParse(parts[0], out int customerId))
        {
            connection.Send(CreateErrorPacket("Invalid customer ID."));
            return;
        }

        Customer? customer = _context.Customers.SingleOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        // Chuẩn hóa dữ liệu
        string name = parts[1].Trim();
        string phone = parts[2].Trim();
        string email = parts[3].Trim();
        string address = parts[4].Trim();
        string taxCode = parts[5].Trim();

        // Cập nhật thông tin khách hàng
        try
        {
            if (!string.IsNullOrEmpty(name)) customer.Name = name;
            if (!string.IsNullOrEmpty(phone)) customer.PhoneNumber = phone;
            if (!string.IsNullOrEmpty(email)) customer.Email = email;
            if (!string.IsNullOrEmpty(address)) customer.Address = address;
            if (!string.IsNullOrEmpty(taxCode)) customer.TaxCode = taxCode;

            _context.SaveChanges();
            connection.Send(CreateSuccessPacket("Customer updated successfully."));
        }
        catch (Exception ex)
        {
            connection.Send(CreateErrorPacket(ex.Message));
            return;
        }
    }

    /// <summary>
    /// Xóa khách hàng khỏi cơ sở dữ liệu (chỉ khi không có đơn hàng liên quan).
    /// </summary>
    [PacketCommand((int)Command.RemoveCustomer, Authoritys.Administrator)]
    public void RemoveCustomer(IPacket packet, IConnection connection)
    {
        if (!TryGetCustomerId(packet, out int customerId))
        {
            connection.Send(CreateErrorPacket("Invalid customer ID."));
            return;
        }

        Customer? customer = _context.Customers.SingleOrDefault(c => c.Id == customerId);
        if (customer == null)
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        // Kiểm tra khách hàng có đơn hàng liên quan không
        if (_context.Customers.Any(o => o.Id == customerId))
        {
            connection.Send(CreateErrorPacket("Cannot remove customer with existing orders."));
            return;
        }

        _context.Customers.Remove(customer);
        _context.SaveChanges();
        connection.Send(CreateSuccessPacket("Customer removed successfully."));
    }

    /// <summary>
    /// Tìm kiếm khách hàng theo từ khóa.
    /// </summary>
    [PacketCommand((int)Command.SearchCustomer, Authoritys.User)]
    public void SearchCustomer(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 3, out string[] parts) ||
            !int.TryParse(parts[1], out int pageIndex) ||
            !int.TryParse(parts[2], out int pageSize))
        {
            connection.Send(CreateErrorPacket("Invalid search parameters."));
            return;
        }

        string keyword = parts[0].Trim();

        var query = _context.Customers.AsNoTracking()
            .Where(c => c.Name.Contains(keyword) ||
                        c.PhoneNumber.Contains(keyword) ||
                        c.Email.Contains(keyword));

        int totalCount = query.Count();
        List<Customer> customers = [.. query
            .OrderBy(c => c.Id)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)];

        if (customers.Count == 0)
        {
            connection.Send(CreateErrorPacket("No customers found."));
            return;
        }

        connection.Send(new Packet(
            PacketType.List, PacketFlags.None, PacketPriority.Low,
            (int)Command.SearchCustomer, ToBytes(customers)).Serialize());
    }

    /// <summary>
    /// Lấy thông tin khách hàng theo ID.
    /// </summary>
    [PacketCommand((int)Command.GetCustomerById, Authoritys.User)]
    public void GetCustomerById(IPacket packet, IConnection connection)
    {
        if (!TryGetCustomerId(packet, out int customerId))
        {
            connection.Send(CreateErrorPacket("Invalid customer ID."));
            return;
        }

        Customer? cus = _context.Customers.AsNoTracking().SingleOrDefault(c => c.Id == customerId);
        if (cus == null)
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        connection.Send(new Packet(PacketFlags.None, PacketPriority.Low,
            (int)Command.GetCustomerById, JsonSerializer.Serialize(cus)).Serialize());
    }

    private static bool TryGetCustomerId(IPacket packet, out int customerId)
    {
        customerId = -1;
        return TryParsePayload(packet, 1, out string[] parts) && int.TryParse(parts[0], out customerId);
    }
}
