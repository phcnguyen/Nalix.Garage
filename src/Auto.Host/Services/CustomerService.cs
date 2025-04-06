using Auto.Common.Entities.Customers;
using Auto.Common.Enums;
using Auto.Database;
using Auto.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Notio.Common.Attributes;
using Notio.Common.Connection;
using Notio.Common.Package;
using Notio.Common.Security;
using Notio.Logging;
using Notio.Network.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auto.Host.Services;

/// <summary>
/// Dịch vụ xử lý thông tin khách hàng, bao gồm thêm, cập nhật, xóa, tìm kiếm và lấy thông tin khách hàng.
/// </summary>
/// <remarks>
/// Khởi tạo CustomerService với DbContext để thao tác dữ liệu.
/// </remarks>
/// <param name="context">Context của cơ sở dữ liệu để thao tác với bảng Customers.</param>
[PacketController]
public sealed class CustomerService(AutoDbContext context) : BaseService
{
    private readonly Repository<Customer> _customerRepository = new(context);

    /// <summary>
    /// Thêm khách hàng mới vào cơ sở dữ liệu.
    /// Định dạng dữ liệu:
    /// - String: "{name}:{phone}:{email}:{address}:{taxCode}:{type}"
    /// - JSON: Customer { Name, PhoneNumber, Email, Address, TaxCode, Type }
    /// Yêu cầu: Phone và Email không được trùng với khách hàng hiện có.
    /// </summary>
    [PacketId((int)Command.AddCustomer)]
    [PacketPermission(PermissionLevel.User)]
    public async Task AddCustomerAsync(IPacket packet, IConnection connection)
    {
        string name, phone, email, address, taxCode;
        CustomerType type;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 6, out string[] parts) || parts.Any(string.IsNullOrWhiteSpace))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid data format."));
                return;
            }

            name = parts[0].Trim();
            phone = parts[1].Trim();
            email = parts[2].Trim();
            address = parts[3].Trim();
            taxCode = parts[4].Trim();
            type = ParseEnum(parts[5], CustomerType.Individual);
        }
        else if (packet.Type == PacketType.Json)
        {
            Customer? customerData = JsonSerializer.Deserialize(packet.Payload.Span, JsonContext.Default.Customer);
            if (customerData == null ||
                string.IsNullOrWhiteSpace(customerData.Name) ||
                string.IsNullOrWhiteSpace(customerData.PhoneNumber) ||
                string.IsNullOrWhiteSpace(customerData.Email))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid JSON data."));
                return;
            }
            name = customerData.Name.Trim();
            phone = customerData.PhoneNumber.Trim();
            email = customerData.Email.Trim();
            address = customerData.Address?.Trim() ?? string.Empty;
            taxCode = customerData.TaxCode?.Trim() ?? string.Empty;
            type = customerData.Type;
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        // Kiểm tra khách hàng đã tồn tại chưa
        if (await _customerRepository.AnyAsync(c => c.PhoneNumber == phone || c.Email == email))
        {
            CLogging.Instance.Warn($"Attempt to add customer with existing phone {phone} or email {email} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Customer with this phone or email already exists."));
            return;
        }

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

            _customerRepository.Add(customer);
            await _customerRepository.SaveChangesAsync();
            CLogging.Instance.Info($"Customer {name} (Phone: {phone}) added successfully by connection {connection.Id}");
            await connection.SendAsync(CreateSuccessPacket("Customer added successfully."));
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to add customer {name} (Phone: {phone}) from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to add customer due to an internal error."));
        }
    }

    /// <summary>
    /// Cập nhật thông tin khách hàng dựa trên ID.
    /// Định dạng dữ liệu:
    /// - String: "{customerId}:{name}:{phone}:{email}:{address}:{taxCode}"
    /// - JSON: Customer { Id, Name, PhoneNumber, Email, Address, TaxCode }
    /// </summary>
    [PacketId((int)Command.UpdateCustomer)]
    [PacketPermission(PermissionLevel.User)]
    public async Task UpdateCustomerAsync(IPacket packet, IConnection connection)
    {
        int customerId;
        string name, phone, email, address, taxCode;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 6, out string[] parts) || !int.TryParse(parts[0], out customerId))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid customer ID or data format."));
                return;
            }

            name = parts[1].Trim();
            phone = parts[2].Trim();
            email = parts[3].Trim();
            address = parts[4].Trim();
            taxCode = parts[5].Trim();
        }
        else if (packet.Type == PacketType.Json)
        {
            Customer? customerData = JsonSerializer.Deserialize(packet.Payload.Span, JsonContext.Default.Customer);
            if (customerData == null || customerData.Id <= 0)
            {
                await connection.SendAsync(CreateErrorPacket("Invalid JSON data or customer ID."));
                return;
            }
            customerId = customerData.Id;
            name = customerData.Name?.Trim() ?? string.Empty;
            phone = customerData.PhoneNumber?.Trim() ?? string.Empty;
            email = customerData.Email?.Trim() ?? string.Empty;
            address = customerData.Address?.Trim() ?? string.Empty;
            taxCode = customerData.TaxCode?.Trim() ?? string.Empty;
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        Customer? customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            CLogging.Instance.Warn($"Attempt to update non-existent customer ID {customerId} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Customer not found."));
            return;
        }

        try
        {
            if (!string.IsNullOrEmpty(name)) customer.Name = name;
            if (!string.IsNullOrEmpty(phone)) customer.PhoneNumber = phone;
            if (!string.IsNullOrEmpty(email)) customer.Email = email;
            if (!string.IsNullOrEmpty(address)) customer.Address = address;
            if (!string.IsNullOrEmpty(taxCode)) customer.TaxCode = taxCode;

            await _customerRepository.SaveChangesAsync();
            CLogging.Instance.Info($"Customer ID {customerId} updated successfully by connection {connection.Id}");
            await connection.SendAsync(CreateSuccessPacket("Customer updated successfully."));
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to update customer ID {customerId} from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to update customer due to an internal error."));
        }
    }

    /// <summary>
    /// Xóa khách hàng khỏi cơ sở dữ liệu (chỉ khi không có đơn hàng liên quan).
    /// Định dạng dữ liệu:
    /// - String: "{customerId}"
    /// - JSON: Customer { Id }
    /// </summary>
    [PacketId((int)Command.RemoveCustomer)]
    [PacketPermission(PermissionLevel.User)]
    public async Task RemoveCustomerAsync(IPacket packet, IConnection connection)
    {
        int customerId;

        if (packet.Type == PacketType.String)
        {
            if (!TryGetCustomerId(packet, out customerId))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid customer ID."));
                return;
            }
        }
        else if (packet.Type == PacketType.Json)
        {
            Customer? customerData = JsonSerializer.Deserialize(packet.Payload.Span, JsonContext.Default.Customer);
            if (customerData == null || customerData.Id <= 0)
            {
                await connection.SendAsync(CreateErrorPacket("Invalid JSON data or customer ID."));
                return;
            }
            customerId = customerData.Id;
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        Customer? customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            CLogging.Instance.Warn($"Attempt to remove non-existent customer ID {customerId} from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Customer not found."));
            return;
        }

        // Kiểm tra khách hàng có đơn hàng liên quan không
        if (await _customerRepository.AnyAsync(o => o.Id == customerId))
        {
            CLogging.Instance.Warn($"Attempt to remove customer ID {customerId} with existing orders from connection {connection.Id}");
            await connection.SendAsync(CreateErrorPacket("Cannot remove customer with existing orders."));
            return;
        }

        try
        {
            _customerRepository.Delete(customer);
            await _customerRepository.SaveChangesAsync();
            CLogging.Instance.Info($"Customer ID {customerId} removed successfully by connection {connection.Id}");
            await connection.SendAsync(CreateSuccessPacket("Customer removed successfully."));
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to remove customer ID {customerId} from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to remove customer due to an internal error."));
        }
    }

    /// <summary>
    /// Tìm kiếm khách hàng theo từ khóa.
    /// Định dạng dữ liệu:
    /// - String: "{keyword}:{pageIndex}:{pageSize}"
    /// - JSON: { "Keyword": string, "PageIndex": int, "PageSize": int }
    /// </summary>
    [PacketId((int)Command.SearchCustomer)]
    [PacketPermission(PermissionLevel.User)]
    public async Task SearchCustomerAsync(IPacket packet, IConnection connection)
    {
        string keyword;
        int pageIndex, pageSize;

        if (packet.Type == PacketType.String)
        {
            if (!TryParsePayload(packet, 3, out string[] parts) ||
                !int.TryParse(parts[1], out pageIndex) ||
                !int.TryParse(parts[2], out pageSize))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid search parameters."));
                return;
            }
            keyword = parts[0].Trim();
        }
        else if (packet.Type == PacketType.Json)
        {
            var searchData = JsonSerializer.Deserialize(packet.Payload.Span, JsonContext.Default.SearchDto);
            if (searchData == null || string.IsNullOrWhiteSpace(searchData.Keyword) || searchData.PageIndex < 0 || searchData.PageSize <= 0)
            {
                await connection.SendAsync(CreateErrorPacket("Invalid JSON search parameters."));
                return;
            }
            keyword = searchData.Keyword.Trim();
            pageIndex = searchData.PageIndex;
            pageSize = searchData.PageSize;
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        try
        {
            var query = _customerRepository.AsQueryable()
                .Where(c => c.Name.Contains(keyword) ||
                            c.PhoneNumber.Contains(keyword) ||
                            c.Email.Contains(keyword));

            int totalCount = await query.CountAsync();
            List<Customer> customers = await query
                .OrderBy(c => c.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (customers.Count == 0)
            {
                CLogging.Instance.Info($"No customers found for keyword '{keyword}' by connection {connection.Id}");
                await connection.SendAsync(CreateErrorPacket("No customers found."));
                return;
            }

            await connection.SendAsync(new Packet((int)Command.SearchCustomer,
                 PacketCode.Success, PacketType.List, PacketFlags.None, PacketPriority.Low,
                 Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customers))).Serialize());
            CLogging.Instance.Info($"Found {customers.Count} customers for keyword '{keyword}' by connection {connection.Id}");
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to search customers with keyword '{keyword}' from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to search customers due to an internal error."));
        }
    }

    /// <summary>
    /// Lấy thông tin khách hàng theo ID.
    /// Định dạng dữ liệu:
    /// - String: "{customerId}"
    /// - JSON: Customer { Id }
    /// </summary>
    [PacketId((int)Command.GetIdByCustomer)]
    [PacketPermission(PermissionLevel.User)]
    public async Task GetCustomerByIdAsync(IPacket packet, IConnection connection)
    {
        int customerId;

        if (packet.Type == PacketType.String)
        {
            if (!TryGetCustomerId(packet, out customerId))
            {
                await connection.SendAsync(CreateErrorPacket("Invalid customer ID."));
                return;
            }
        }
        else if (packet.Type == PacketType.Json)
        {
            Customer? customerData = JsonSerializer.Deserialize(packet.Payload.Span, JsonContext.Default.Customer);
            if (customerData == null || customerData.Id <= 0)
            {
                await connection.SendAsync(CreateErrorPacket("Invalid JSON data or customer ID."));
                return;
            }
            customerId = customerData.Id;
        }
        else
        {
            await connection.SendAsync(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        try
        {
            Customer? customer = await _customerRepository.AsQueryable().SingleOrDefaultAsync(c => c.Id == customerId);
            if (customer == null)
            {
                CLogging.Instance.Warn($"Customer ID {customerId} not found by connection {connection.Id}");
                await connection.SendAsync(CreateErrorPacket("Customer not found."));
                return;
            }

            await connection.SendAsync(new Packet((int)Command.GetIdByCustomer,
                PacketCode.Success, PacketType.Json, PacketFlags.None, PacketPriority.Low,
                Encoding.UTF8.GetBytes(JsonSerializer.Serialize(customer))).Serialize());

            CLogging.Instance.Info($"Customer ID {customerId} retrieved successfully by connection {connection.Id}");
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to retrieve customer ID {customerId} from connection {connection.Id}", ex);
            await connection.SendAsync(CreateErrorPacket("Failed to retrieve customer due to an internal error."));
        }
    }

    /// <summary>
    /// Kiểm tra và lấy ID khách hàng từ gói tin dạng String.
    /// </summary>
    private static bool TryGetCustomerId(IPacket packet, out int customerId)
    {
        customerId = -1;
        return TryParsePayload(packet, 1, out string[] parts) && int.TryParse(parts[0], out customerId);
    }
}