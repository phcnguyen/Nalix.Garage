using Auto.Common.Entities.Customers;
using Auto.Common.Enums;
using Auto.Database;
using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Common.Package;
using Notio.Network.Handlers;
using System;

namespace Auto.Server.Services;

[PacketController]
public sealed class CustomerService(AutoDbContext context) : BaseService
{
    private readonly AutoDbContext context = context;

    [PacketCommand((int)Command.AddCustomer, Authoritys.User)]
    public void AddCustomer(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 6, out string[] parts))
        {
            connection.Send(PacketDefault);
            return;
        }

        Customer customer = new()
        {
            Name = parts[0],
            PhoneNumber = parts[1],
            Email = parts[2],
            Address = parts[3],
            TaxCode = parts[4],
            Type = Enum.TryParse(parts[5], true, out CustomerType t) ? t : CustomerType.Individual
        };

        context.Customers.Add(customer);
        context.SaveChanges();
    }

    [PacketCommand((int)Command.UpdateCustomer, Authoritys.User)]
    public void UpdateCustomer(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 6, out string[] parts) || !int.TryParse(parts[0], out int customerId))
        {
            connection.Send(PacketDefault);
            return;
        }

        Customer? customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            connection.Send(PacketDefault);
            return;
        }

        customer.Name = parts[1];
        customer.PhoneNumber = parts[2];
        customer.Email = parts[3];
        customer.Address = parts[4];
        customer.TaxCode = parts[5];

        context.SaveChanges();
    }

    [PacketCommand((int)Command.RemoveCustomer, Authoritys.User)]
    public void RemoveCustomer(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 1, out string[] parts) || !int.TryParse(parts[0], out int customerId))
        {
            connection.Send(PacketDefault);
            return;
        }

        Customer? customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            connection.Send(PacketDefault);
            return;
        }

        context.Customers.Remove(customer);
        context.SaveChanges();
    }
}
