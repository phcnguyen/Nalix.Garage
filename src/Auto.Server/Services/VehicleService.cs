using Auto.Common.Entities.Customers;
using Auto.Common.Entities.Vehicles;
using Auto.Common.Enums;
using Auto.Common.Enums.Cars;
using Auto.Database;
using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Common.Package;
using Notio.Network.Handlers;

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
    /// <param name="packet">Gói tin chứa thông tin phương tiện.</param>
    /// <param name="connection">Kết nối của client gửi yêu cầu.</param>
    [PacketCommand((int)Command.AddVehicle, Authoritys.User)]
    public void AddVehicle(IPacket packet, IConnection connection)
    {
        if (!TryParsePayload(packet, 9, out string[] parts) || !int.TryParse(parts[0], out int customerId))
        {
            connection.Send(CreateErrorPacket("Invalid data format."));
            return;
        }

        Customer? customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            connection.Send(CreateErrorPacket("Customer not found."));
            return;
        }

        Vehicle vehicle = new()
        {
            CustomerId = customer.CustomerId,
            CarYear = ParseInt(parts[1], 1900),
            CarType = ParseEnum(parts[2], CarType.None),
            CarColor = ParseEnum(parts[3], CarColor.None),
            CarBrand = ParseEnum(parts[4], CarBrand.None),
            CarLicensePlate = parts[5],
            CarModel = parts[6],
            FrameNumber = parts[7],
            EngineNumber = parts[8],
            Mileage = ParseInt(parts[9], 0)
        };

        context.Vehicles.Add(vehicle);
        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Vehicle added successfully."));
    }

    /// <summary>
    /// Cập nhật thông tin phương tiện trong hệ thống.
    /// </summary>
    /// <param name="packet">Gói tin chứa thông tin cập nhật.</param>
    /// <param name="connection">Kết nối của client gửi yêu cầu.</param>
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

        vehicle.CarYear = ParseInt(parts[1], vehicle.CarYear);
        vehicle.CarType = ParseEnum(parts[2], vehicle.CarType);
        vehicle.CarColor = ParseEnum(parts[3], vehicle.CarColor);
        vehicle.CarBrand = ParseEnum(parts[4], vehicle.CarBrand);
        vehicle.CarLicensePlate = parts[5];
        vehicle.CarModel = parts[6];
        vehicle.FrameNumber = parts[7];
        vehicle.EngineNumber = parts[8];
        vehicle.Mileage = ParseDouble(parts[9], vehicle.Mileage);

        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Vehicle updated successfully."));
    }

    /// <summary>
    /// Xóa một phương tiện khỏi hệ thống.
    /// </summary>
    /// <param name="packet">Gói tin chứa ID phương tiện cần xóa.</param>
    /// <param name="connection">Kết nối của client gửi yêu cầu.</param>
    [PacketCommand((int)Command.RemoveVehicle, Authoritys.User)]
    public void RemoveVehicle(IPacket packet, IConnection connection)
    {
        if (!TryGetVehicleId(packet, out int vehicleId))
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

        context.Vehicles.Remove(vehicle);
        context.SaveChanges();
        connection.Send(CreateSuccessPacket("Vehicle removed successfully."));
    }

    /// <summary>
    /// Kiểm tra và lấy ID phương tiện từ gói tin.
    /// </summary>
    /// <param name="packet">Gói tin chứa thông tin ID.</param>
    /// <param name="vehicleId">ID phương tiện nếu hợp lệ.</param>
    /// <returns>Trả về true nếu ID hợp lệ, ngược lại trả về false.</returns>
    private static bool TryGetVehicleId(IPacket packet, out int vehicleId)
    {
        vehicleId = -1;
        return TryParsePayload(packet, 1, out string[] parts) && int.TryParse(parts[0], out vehicleId);
    }
}
