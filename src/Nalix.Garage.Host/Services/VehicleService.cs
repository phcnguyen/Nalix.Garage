﻿using Nalix.Common.Connection;
using Nalix.Common.Package;
using Nalix.Common.Package.Attributes;
using Nalix.Common.Package.Enums;
using Nalix.Common.Security;
using Nalix.Garage.Common;
using Nalix.Garage.Common.Entities.Customers;
using Nalix.Garage.Common.Entities.Vehicles;
using Nalix.Garage.Common.Enums;
using Nalix.Garage.Common.Enums.Cars;
using Nalix.Garage.Database;
using Nalix.Garage.Database.Repositories;
using Nalix.Logging;
using Nalix.Serialization;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Nalix.Garage.Host.Services;

/// <summary>
/// Dịch vụ quản lý thông tin phương tiện của khách hàng.
/// </summary>
public sealed class VehicleService(AutoDbContext context) : BaseService
{
    private readonly Repository<Vehicle> _vehicleRepository = new(context);
    private readonly Repository<Customer> _customerRepository = new(context);

    /// <summary>
    /// Thêm một phương tiện mới vào hệ thống.
    /// Định dạng dữ liệu:
    /// - String: "{customerId}:{carYear}:{carType}:{carColor}:{carBrand}:{licensePlate}:{carModel}:{frameNumber}:{engineNumber}:{mileage}"
    /// - JSON: Vehicle { CustomerId, CarYear, CarType, CarColor, CarBrand, CarLicensePlate, CarModel, FrameNumber, EngineNumber, Mileage }
    /// Yêu cầu: CustomerId hợp lệ, LicensePlate/FrameNumber/EngineNumber không trùng lặp.
    /// </summary>
    /// <param name="packet">Gói dữ liệu chứa thông tin phương tiện.</param>
    /// <param name="connection">Kết nối với client để gửi phản hồi.</param>
    /// <returns>Task đại diện cho quá trình xử lý bất đồng bộ.</returns>
    [PacketId((int)Command.AddVehicle)]
    [PacketPermission(PermissionLevel.User)]
    public async Task AddVehicleAsync(IPacket packet, IConnection connection)
    {
        int customerId;
        int carYear;
        CarType carType;
        CarColor carColor;
        CarBrand carBrand;
        string carModel, licensePlate, frameNumber, engineNumber;
        double mileage;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 10, out string[] parts) || parts.Any(string.IsNullOrWhiteSpace))
            {
                await connection.SendAsync(InvalidDataPacket());
                return;
            }

            if (!int.TryParse(parts[0], out customerId))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid customer ID."));
                return;
            }

            carYear = ParseInt(parts[1], 1900);
            carType = ParseEnum(parts[2], CarType.None);
            carColor = ParseEnum(parts[3], CarColor.None);
            carBrand = ParseEnum(parts[4], CarBrand.None);
            licensePlate = parts[5].Trim();
            carModel = parts[6].Trim();
            frameNumber = parts[7].Trim();
            engineNumber = parts[8].Trim();
            mileage = ParseDouble(parts[9], 0);
        }
        else if (packet.Type == PacketType.Json)
        {
            Vehicle? vehicleData = JsonSerializer.Deserialize<Vehicle>(packet.Payload.Span);
            if (vehicleData == null || string.IsNullOrWhiteSpace(vehicleData.CarLicensePlate) ||
                string.IsNullOrWhiteSpace(vehicleData.CarModel) || string.IsNullOrWhiteSpace(vehicleData.FrameNumber) ||
                string.IsNullOrWhiteSpace(vehicleData.EngineNumber))
            {
                await connection.SendAsync(InvalidDataPacket());
                return;
            }
            customerId = vehicleData.CustomerId;
            carYear = vehicleData.CarYear;
            carType = vehicleData.CarType;
            carColor = vehicleData.CarColor;
            carBrand = vehicleData.CarBrand;
            licensePlate = vehicleData.CarLicensePlate.Trim();
            carModel = vehicleData.CarModel.Trim();
            frameNumber = vehicleData.FrameNumber.Trim();
            engineNumber = vehicleData.EngineNumber.Trim();
            mileage = vehicleData.Mileage;
        }
        else
        {
            await connection.SendAsync(InvalidDataPacket());
            return;
        }

        // Kiểm tra quyền sở hữu (giả sử Metadata lưu CustomerId của người dùng)
        if (!connection.Metadata.TryGetValue("CustomerId", out object? value) ||
            value is not int userCustomerId || userCustomerId != customerId)
        {
            await connection.SendAsync(CreateErrorPacket(
                "You are not authorized to add a vehicle for this customer."));
            return;
        }

        if (!await _customerRepository.AnyAsync(c => c.Id == customerId))
        {
            await connection.SendAsync(CreateErrorPacket("Customer not found."));
            return;
        }

        if (await _vehicleRepository.AnyAsync(v => v.CarLicensePlate == licensePlate))
        {
            await connection.SendAsync(CreateErrorPacket("Vehicle already exists with the same license plate."));
            return;
        }
        if (await _vehicleRepository.AnyAsync(v => v.FrameNumber == frameNumber))
        {
            await connection.SendAsync(CreateErrorPacket("Vehicle already exists with the same frame number."));
            return;
        }
        if (await _vehicleRepository.AnyAsync(v => v.EngineNumber == engineNumber))
        {
            await connection.SendAsync(CreateErrorPacket("Vehicle already exists with the same engine number."));
            return;
        }

        Vehicle vehicle = new()
        {
            CustomerId = customerId,
            CarYear = carYear,
            CarType = carType,
            CarColor = carColor,
            CarBrand = carBrand,
            CarLicensePlate = licensePlate,
            CarModel = carModel,
            FrameNumber = frameNumber,
            EngineNumber = engineNumber,
            Mileage = mileage
        };

        try
        {
            _vehicleRepository.Add(vehicle);
            await _vehicleRepository.SaveChangesAsync();
            NLogix.Host.Instance.Info($"Vehicle added successfully for CustomerId: {customerId}");
            await connection.SendAsync(CreateSuccessPacket("Vehicle added successfully."));
        }
        catch (Exception ex)
        {
            NLogix.Host.Instance.Error($"Failed to add vehicle for CustomerId: {customerId}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to add vehicle due to an internal error."));
        }
    }

    /// <summary>
    /// Cập nhật thông tin phương tiện trong hệ thống.
    /// Định dạng dữ liệu:
    /// - String: "{vehicleId}:{carYear}:{carType}:{carColor}:{carBrand}:{licensePlate}:{carModel}:{frameNumber}:{engineNumber}:{mileage}"
    /// - JSON: Vehicle { Id, CarYear, CarType, CarColor, CarBrand, CarLicensePlate, CarModel, FrameNumber, EngineNumber, Mileage }
    /// Yêu cầu: VehicleId tồn tại, LicensePlate/FrameNumber/EngineNumber không trùng với xe khác.
    /// </summary>
    /// <param name="packet">Gói dữ liệu chứa thông tin cập nhật.</param>
    /// <param name="connection">Kết nối với client để gửi phản hồi.</param>
    /// <returns>Task đại diện cho quá trình xử lý bất đồng bộ.</returns>
    [PacketId((int)Command.UpdateVehicle)]
    [PacketPermission(PermissionLevel.User)]
    public async Task UpdateVehicleAsync(IPacket packet, IConnection connection)
    {
        int vehicleId;
        int carYear;
        CarType carType;
        CarColor carColor;
        CarBrand carBrand;
        string carModel, licensePlate, frameNumber, engineNumber;
        double mileage;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 10, out string[] parts) || parts.Any(string.IsNullOrWhiteSpace))
            {
                await connection.SendAsync(InvalidDataPacket());
                return;
            }

            if (!int.TryParse(parts[0], out vehicleId))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid vehicle ID."));
                return;
            }

            carYear = ParseInt(parts[1], 1900); // Giá trị mặc định nếu không parse được
            carType = ParseEnum(parts[2], CarType.None);
            carColor = ParseEnum(parts[3], CarColor.None);
            carBrand = ParseEnum(parts[4], CarBrand.None);
            licensePlate = parts[5].Trim();
            carModel = parts[6].Trim();
            frameNumber = parts[7].Trim();
            engineNumber = parts[8].Trim();
            mileage = ParseDouble(parts[9], 0);
        }
        else if (packet.Type == PacketType.Json)
        {
            Vehicle? vehicleData = JsonCodec.DeserializeFromBytes(
                packet.Payload.Span, JsonContext.Default.Vehicle);
            if (vehicleData == null ||
                string.IsNullOrWhiteSpace(vehicleData.CarLicensePlate) ||
                string.IsNullOrWhiteSpace(vehicleData.CarModel) ||
                string.IsNullOrWhiteSpace(vehicleData.FrameNumber) ||
                string.IsNullOrWhiteSpace(vehicleData.EngineNumber))
            {
                await connection.SendAsync(InvalidDataPacket());
                return;
            }
            vehicleId = vehicleData.Id;
            carYear = vehicleData.CarYear;
            carType = vehicleData.CarType;
            carColor = vehicleData.CarColor;
            carBrand = vehicleData.CarBrand;
            licensePlate = vehicleData.CarLicensePlate.Trim();
            carModel = vehicleData.CarModel.Trim();
            frameNumber = vehicleData.FrameNumber.Trim();
            engineNumber = vehicleData.EngineNumber.Trim();
            mileage = vehicleData.Mileage;
        }
        else
        {
            await connection.SendAsync(InvalidDataPacket());
            return;
        }

        Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
        if (vehicle == null)
        {
            await connection.SendAsync(CreateErrorPacket("Vehicle not found."));
            return;
        }

        // Kiểm tra quyền sở hữu
        if (!connection.Metadata.TryGetValue("CustomerId", out object? value) || value is not int userCustomerId || userCustomerId != vehicle.CustomerId)
        {
            await connection.SendAsync(CreateErrorPacket("You are not authorized to update this vehicle."));
            return;
        }

        if (await _vehicleRepository.AnyAsync(v =>
            v.Id != vehicleId &&
            (v.CarLicensePlate == licensePlate || v.FrameNumber == frameNumber || v.EngineNumber == engineNumber)))
        {
            await connection.SendAsync(CreateErrorPacket("Another vehicle already exists with the same license plate, frame number, or engine number."));
            return;
        }

        try
        {
            vehicle.CarYear = carYear;
            vehicle.CarType = carType;
            vehicle.CarColor = carColor;
            vehicle.CarBrand = carBrand;
            vehicle.CarLicensePlate = licensePlate;
            vehicle.CarModel = carModel;
            vehicle.FrameNumber = frameNumber;
            vehicle.EngineNumber = engineNumber;
            vehicle.Mileage = mileage;

            await _vehicleRepository.SaveChangesAsync();
            NLogix.Host.Instance.Info($"Vehicle updated successfully. VehicleId: {vehicleId}");
            await connection.SendAsync(CreateSuccessPacket("Vehicle updated successfully."));
        }
        catch (Exception ex)
        {
            NLogix.Host.Instance.Error($"Failed to update vehicle. VehicleId: {vehicleId}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to update vehicle due to an internal error."));
        }
    }

    /// <summary>
    /// Xóa một phương tiện khỏi hệ thống (chỉ dành cho quản trị viên).
    /// Định dạng dữ liệu:
    /// - String: "{vehicleId}"
    /// - JSON: Vehicle { Id }
    /// </summary>
    /// <param name="packet">Gói dữ liệu chứa ID phương tiện cần xóa.</param>
    /// <param name="connection">Kết nối với client để gửi phản hồi.</param>
    /// <returns>Task đại diện cho quá trình xử lý bất đồng bộ.</returns>
    [PacketId((int)Command.RemoveVehicle)]
    [PacketPermission(PermissionLevel.User)]
    public async Task RemoveVehicleAsync(IPacket packet, IConnection connection)
    {
        int vehicleId;

        if (packet.Type == PacketType.String)
        {
            if (!TryGetVehicleId(packet, out vehicleId))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid vehicle ID."));
                return;
            }
        }
        else if (packet.Type == PacketType.Json)
        {
            Vehicle? vehicleData = JsonSerializer.Deserialize(
                packet.Payload.Span, JsonContext.Default.Vehicle);
            if (vehicleData == null || vehicleData.Id <= 0)
            {
                await connection.SendAsync(InvalidDataPacket());
                return;
            }
            vehicleId = vehicleData.Id;
        }
        else
        {
            await connection.SendAsync(InvalidDataPacket());
            return;
        }

        try
        {
            if (_vehicleRepository.Exists(vehicleId))
            {
                _vehicleRepository.Delete(vehicleId);
                NLogix.Host.Instance.Info($"Vehicle removed successfully. VehicleId: {vehicleId}");
                await connection.SendAsync(CreateSuccessPacket("Vehicle removed successfully."));
            }
            else
            {
                await connection.SendAsync(CreateErrorPacket("Vehicle not found."));
            }
        }
        catch (Exception ex)
        {
            NLogix.Host.Instance.Error($"Failed to remove vehicle. VehicleId: {vehicleId}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to remove vehicle due to an internal error."));
        }
    }

    /// <summary>
    /// Kiểm tra và lấy ID phương tiện từ gói tin.
    /// </summary>
    private static bool TryGetVehicleId(IPacket packet, out int vehicleId)
    {
        vehicleId = -1;
        return TryParsePayload(packet, 1, out string[] parts) &&
               int.TryParse(parts[0], out vehicleId);
    }
}