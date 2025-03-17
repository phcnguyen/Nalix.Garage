using Auto.Common.Enums;
using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Network.Package.Extensions;
using System;
using System.Buffers.Text;
using System.Runtime.InteropServices;
using System.Text;

namespace Auto.Host.Services.Base;

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
    protected static byte[] CreateErrorPacket(string message)
        => new Packet(PacketFlags.None, PacketPriority.None, (ushort)Command.Error, message).Serialize();

    /// <summary>
    /// Tạo gói tin chứa lỗi.
    /// </summary>
    protected static byte[] InvalidDataPacket()
        => CreateErrorPacket("Invalid data format.");

    /// <summary>
    /// Tạo gói tin phản hồi thành công.
    /// </summary>
    protected static byte[] CreateSuccessPacket(string message)
        => new Packet(PacketFlags.None, PacketPriority.None, (ushort)Command.Success, message).Serialize();

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
