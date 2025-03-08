using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Vehicles;
using Auto.Common.Enums;
using Auto.Common.Enums.Cars;
using Auto.Database;
using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Common.Package;
using Notio.Network.Handlers;
using System;

namespace Auto.Server.Services;

public sealed class VehicleService(AutoDbContext context) : BaseService
{
    private readonly AutoDbContext context = context;

    [PacketCommand((int)Command.AddVehicle, Authoritys.User)]
    public void AddVehicle(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 9, out string[] parts))
        {
            connection.Send(PacketDefault);
            return;
        }

        if (!int.TryParse(parts[0], out int customerId))
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

        Vehicle vehicle = new()
        {
            CustomerId = customer.CustomerId,
            CarYear = int.TryParse(parts[1], out int year) ? year : 1900,
            CarType = Enum.TryParse(parts[2], true, out CarType carType) ? carType : CarType.None,
            CarColor = Enum.TryParse(parts[3], true, out CarColor carColor) ? carColor : CarColor.None,
            CarBrand = Enum.TryParse(parts[4], true, out CarBrand carBrand) ? carBrand : CarBrand.None,
            CarLicensePlate = parts[5],
            CarModel = parts[6],
            FrameNumber = parts[7],
            EngineNumber = parts[8],
            Mileage = int.TryParse(parts[9], out int mileage) ? mileage : 0
        };

        context.Vehicles.Add(vehicle);
        context.SaveChanges();
    }

    [PacketCommand((int)Command.UpdateVehicle, Authoritys.User)]
    public void UpdateVehicle(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 10, out string[] parts) || !int.TryParse(parts[0], out int vehicleId))
        {
            connection.Send(PacketDefault);
            return;
        }

        Vehicle? vehicle = context.Vehicles.Find(vehicleId);
        if (vehicle == null)
        {
            connection.Send(PacketDefault);
            return;
        }

        vehicle.CarYear = int.TryParse(parts[1], out int year) ? year : vehicle.CarYear;
        vehicle.CarType = Enum.TryParse(parts[2], true, out CarType carType) ? carType : vehicle.CarType;
        vehicle.CarColor = Enum.TryParse(parts[3], true, out CarColor carColor) ? carColor : vehicle.CarColor;
        vehicle.CarBrand = Enum.TryParse(parts[4], true, out CarBrand carBrand) ? carBrand : vehicle.CarBrand;
        vehicle.CarLicensePlate = parts[5];
        vehicle.CarModel = parts[6];
        vehicle.FrameNumber = parts[7];
        vehicle.EngineNumber = parts[8];
        vehicle.Mileage = int.TryParse(parts[9], out int mileage) ? mileage : vehicle.Mileage;

        context.SaveChanges();
    }

    [PacketCommand((int)Command.RemoveVehicle, Authoritys.User)]
    public void RemoveVehicle(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 1, out string[] parts) || !int.TryParse(parts[0], out int vehicleId))
        {
            connection.Send(PacketDefault);
            return;
        }

        Vehicle? vehicle = context.Vehicles.Find(vehicleId);
        if (vehicle == null)
        {
            connection.Send(PacketDefault);
            return;
        }

        context.Vehicles.Remove(vehicle);
        context.SaveChanges();
    }
}
