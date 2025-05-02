using Nalix.Common.Package;
using Nalix.Common.Package.Enums;
using Nalix.Network.Package;
using System;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Text;

namespace Nalix.Garage.Host.Services;

/// <summary>
/// Lớp cơ sở cung cấp các phương thức hỗ trợ cho các dịch vụ xử lý gói tin.
/// </summary>
public abstract class BaseService
{
    /// <summary>
    /// Phân tích payload của gói tin thành một mảng chuỗi dựa trên ký tự phân cách '|'.
    /// </summary>
    protected static bool TryParsePayload(IPacket packet, int expectedParts, out string[] parts)
    {
        string payload = Encoding.UTF8.GetString(packet.Payload.Span);
        parts = payload.Split('|', StringSplitOptions.TrimEntries);
        return parts.Length >= expectedParts;
    }

    /// <summary>
    /// Tạo gói tin chứa lỗi.
    /// </summary>
    protected static Memory<byte> CreateErrorPacket(string message)
        => new Packet(0, PacketCode.UnknownError, PacketFlags.None, PacketPriority.Low, message).Serialize();

    /// <summary>
    /// Tạo gói tin chứa lỗi.
    /// </summary>
    protected static Memory<byte> InvalidDataPacket()
        => CreateErrorPacket("Invalid data format.");

    /// <summary>
    /// Tạo gói tin phản hồi thành công.
    /// </summary>
    protected static Memory<byte> CreateSuccessPacket(string message)
        => new Packet(0, PacketCode.Success, PacketFlags.None, PacketPriority.Low, message).Serialize();

    /// <summary>
    /// Chuyển đổi chuỗi thành số nguyên, nếu không hợp lệ thì trả về giá trị mặc định.
    /// </summary>
    protected static int ParseInt(ReadOnlySpan<char> value, int defaultValue)
    {
        if (Utf8Parser.TryParse(MemoryMarshal.AsBytes(value), out int result, out _))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Chuyển đổi chuỗi thành số thực, nếu không hợp lệ thì trả về giá trị mặc định.
    /// </summary>
    protected static double ParseDouble(ReadOnlySpan<char> value, double defaultValue)
    {
        if (Utf8Parser.TryParse(MemoryMarshal.AsBytes(value), out double result, out _))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Chuyển đổi chuỗi thành kiểu enum, nếu không hợp lệ thì trả về giá trị mặc định.
    /// </summary>
    protected static T ParseEnum<T>(ReadOnlySpan<char> value, T defaultValue)
        where T : struct, Enum
    {
        if (Enum.TryParse(value, true, out T result))
            return result;

        return defaultValue;
    }

    /// <summary>
    /// Thử chuyển đổi chuỗi thành enum, nếu hợp lệ thì trả về giá trị enum, nếu không thì trả về false.
    /// </summary>
    protected static bool TryParseEnum<T>(ReadOnlySpan<char> value, out T result)
        where T : struct, Enum
        => Enum.TryParse(value, true, out result);
}