using Auto.Common.Entities.Vehicles;
using Auto.Common.Enums;
using Auto.Common.Enums.Cars;
using Auto.Database;
using Auto.Server.Services.Base;
using Microsoft.EntityFrameworkCore;
using Notio.Common.Attributes;
using Notio.Common.Connection;
using Notio.Common.Enums;
using Notio.Common.Interfaces;
using System;
using System.Linq;

namespace Auto.Server.Services;

/// <summary>
/// Dịch vụ quản lý thông tin phương tiện của khách hàng.
/// </summary>
public sealed class VehicleService(AutoDbContext context) : BaseService
{
    private readonly AutoDbContext context = context;

    /// <summary>
    /// Thêm một phương tiện mới vào hệ thống.
    /// </summary>
    [PacketCommand((int)Command.AddVehicle, Authoritys.User)]
    public void AddVehicle(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 9, out string[] parts) || !int.TryParse(parts[0], out int customerId))
        {
            connection.Send(CreateErrorPacket("Invalid data format."));
            return;
        }

        if (!context.Customers.Any(c => c.Id == customerId))
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        string licensePlate = parts[5].Trim();
        string frameNumber = parts[7].Trim();
        string engineNumber = parts[8].Trim();

        if (context.Vehicles.Any(v => v.CarLicensePlate == licensePlate || v.FrameNumber == frameNumber || v.EngineNumber == engineNumber))
        {
            connection.Send(CreateErrorPacket("Vehicle already exists with the same license plate, frame number, or engine number."));
            return;
        }

        Vehicle vehicle = new()
        {
            CustomerId = customerId,
            CarYear = ParseInt(parts[1], 1900),
            CarType = ParseEnum(parts[2], CarType.None),
            CarColor = ParseEnum(parts[3], CarColor.None),
            CarBrand = ParseEnum(parts[4], CarBrand.None),
            CarLicensePlate = licensePlate,
            CarModel = parts[6].Trim(),
            FrameNumber = frameNumber,
            EngineNumber = engineNumber,
            Mileage = ParseInt(parts[9], 0)
        };

        context.Vehicles.Add(vehicle);
        try
        {
            context.SaveChanges();
            connection.Send(CreateSuccessPacket("Vehicle added successfully."));
        }
        catch (Exception)
        {
            connection.Send(CreateErrorPacket("Failed to add vehicle."));
        }
    }

    /// <summary>
    /// Cập nhật thông tin phương tiện trong hệ thống.
    /// </summary>
    [PacketCommand((int)Command.UpdateVehicle, Authoritys.User)]
    public void UpdateVehicle(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 10, out string[] parts) || !int.TryParse(parts[0], out int vehicleId))
        {
            connection.Send(CreateErrorPacket("Invalid vehicle ID."));
            return;
        }

        Vehicle? vehicle = context.Vehicles.Find(vehicleId);
        if (vehicle == null)
        {
            connection.Send(CreateErrorPacket("Vehicle not found."));
            return;
        }

        string licensePlate = parts[5].Trim();
        string frameNumber = parts[7].Trim();
        string engineNumber = parts[8].Trim();

        if (context.Vehicles.Any(v =>
            v.Id != vehicleId &&
            (v.CarLicensePlate == licensePlate ||
            v.FrameNumber == frameNumber ||
            v.EngineNumber == engineNumber)))
        {
            connection.Send(
                CreateErrorPacket("Another vehicle already exists with the same license plate, frame number, or engine number."));
            return;
        }

        try
        {
            context.Attach(vehicle); // Tối ưu cập nhật chỉ những trường thay đổi
            vehicle.CarYear = ParseInt(parts[1], vehicle.CarYear);
            vehicle.CarType = ParseEnum(parts[2], vehicle.CarType);
            vehicle.CarColor = ParseEnum(parts[3], vehicle.CarColor);
            vehicle.CarBrand = ParseEnum(parts[4], vehicle.CarBrand);
            vehicle.CarLicensePlate = licensePlate;
            vehicle.CarModel = parts[6].Trim();
            vehicle.FrameNumber = frameNumber;
            vehicle.EngineNumber = engineNumber;
            vehicle.Mileage = ParseDouble(parts[9], vehicle.Mileage);

            context.SaveChanges();
            connection.Send(CreateSuccessPacket("Vehicle updated successfully."));
        }
        catch (Exception)
        {
            connection.Send(CreateErrorPacket("Failed to update vehicle."));
        }
    }

    /// <summary>
    /// Xóa một phương tiện khỏi hệ thống.
    /// </summary>
    [PacketCommand((int)Command.RemoveVehicle, Authoritys.Administrator)]
    public void RemoveVehicle(IPacket packet, IConnection connection)
    {
        if (!TryGetVehicleId(packet, out int vehicleId))
        {
            connection.Send(CreateErrorPacket("Invalid vehicle ID."));
            return;
        }

        // Tối ưu bằng cách xóa trực tiếp, tránh truy vấn thừa
        int deleted = context.Vehicles.Where(v => v.Id == vehicleId).ExecuteDelete();

        if (deleted > 0)
            connection.Send(CreateSuccessPacket("Vehicle removed successfully."));
        else
            connection.Send(CreateErrorPacket("Vehicle not found."));
    }

    /// <summary>
    /// Kiểm tra và lấy ID phương tiện từ gói tin.
    /// </summary>
    private static bool TryGetVehicleId(IPacket packet, out int vehicleId)
    {
        vehicleId = -1;
        return TryParsePayload(packet, 1, out string[] parts) && int.TryParse(parts[0], out vehicleId);
    }
}
