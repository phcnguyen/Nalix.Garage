using Auto.Common.Entities.Customers;
using Auto.Common.Enums;
using Auto.Database;
using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Common.Package;
using Notio.Network.Handlers;
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
public sealed class CustomerService(AutoDbContext context) : BaseService
{
    private readonly AutoDbContext context = context;

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

        if (context.Customers.Any(c => c.PhoneNumber == parts[1] || c.Email == parts[2]))
        {
            connection.Send(CreateErrorPacket("Customer with this phone or email already exists."));
            return;
        }

        Customer customer = new()
        {
            Name = parts[0],
            PhoneNumber = parts[1],
            Email = parts[2],
            Address = parts[3],
            TaxCode = parts[4],
            Type = ParseEnum(parts[4], CustomerType.Individual)
        };

        context.Customers.Add(customer);
        context.SaveChanges();

        connection.Send(CreateSuccessPacket("Customer added successfully."));
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

        Customer? customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        customer.Name = parts[1];
        customer.PhoneNumber = parts[2];
        customer.Email = parts[3];
        customer.Address = parts[4];
        customer.TaxCode = parts[5];

        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Customer updated successfully."));
    }

    /// <summary>
    /// Xóa khách hàng khỏi cơ sở dữ liệu.
    /// </summary>
    [PacketCommand((int)Command.RemoveCustomer, Authoritys.User)]
    public void RemoveCustomer(IPacket packet, IConnection connection)
    {
        if (!TryGetCustomerId(packet, out int customerId))
        {
            connection.Send(CreateErrorPacket("Invalid customer ID."));
            return;
        }

        Customer? customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        context.Customers.Remove(customer);
        context.SaveChanges();
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

        var query = context.Customers
            .Where(c => c.Name.Contains(keyword) ||
                        c.PhoneNumber.Contains(keyword) ||
                        c.Email.Contains(keyword));

        int totalCount = query.Count();
        List<Customer> customers = [.. query
            .OrderBy(c => c.CustomerId)
            .Skip(pageIndex * pageSize)
            .Take(pageSize)];

        if (customers.Count == 0)
        {
            connection.Send(CreateErrorPacket("No customers found."));
            return;
        }

        connection.Send(new Packet(
            PacketType.List, PacketFlags.None, PacketPriority.Low,
            (int)Command.SearchCustomer, ToBytes<Customer>(customers)).Serialize());
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

        Customer? cus = context.Customers.Find(customerId);
        if (cus == null)
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        connection.Send(new Packet(PacketFlags.None, PacketPriority.Low,
            (int)Command.GetCustomerById, JsonSerializer.Serialize(cus)).Serialize());
    }

    /// <summary>
    /// Kiểm tra và lấy ID khách hàng từ gói tin.
    /// </summary>
    /// <param name="packet">Gói tin chứa thông tin ID.</param>
    /// <param name="customerId">Giá trị ID khách hàng sau khi phân tích.</param>
    /// <returns>Trả về <c>true</c> nếu lấy ID thành công, <c>false</c> nếu thất bại.</returns>
    private static bool TryGetCustomerId(IPacket packet, out int customerId)
    {
        customerId = -1;
        return TryParsePayload(packet, 1, out string[] parts) && int.TryParse(parts[0], out customerId);
    }
}
