using Auto.Common.Enums;
using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Network.Package.Enums;
using Notio.Network.Package.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Auto.Server.Services;

/// <summary>
/// Lớp cơ sở cung cấp các phương thức hỗ trợ cho các dịch vụ xử lý gói tin.
/// </summary>
public abstract class BaseService
{
    /// <summary>
    /// Gói tin mặc định, sử dụng khi không có phản hồi cụ thể.
    /// Gói này có kiểu Binary, không có cờ hoặc mức độ ưu tiên, với payload chứa một byte 0.
    /// </summary>
    protected static readonly byte[] DefaultPacket = new Packet(
        PacketType.Binary, PacketFlags.None, PacketPriority.None, 0, new byte[] { 0 }).Serialize();

    /// <summary>
    /// Gói tin lỗi mặc định, sử dụng khi xảy ra lỗi không xác định.
    /// Gói này chứa thông báo "An error has occurred".
    /// </summary>
    protected static readonly byte[] DefaultErrorPacket = new Packet(
        PacketFlags.None, PacketPriority.None, (int)Command.Error, "An error has occurred").Serialize();

    /// <summary>
    /// Phân tích payload của gói tin thành một mảng chuỗi dựa trên ký tự phân cách '|'.
    /// </summary>
    /// <param name="packet">Gói tin chứa payload cần phân tích.</param>
    /// <param name="expectedParts">Số lượng phần tử tối thiểu mong đợi sau khi tách.</param>
    /// <param name="parts">Mảng chứa các phần tử sau khi tách từ payload.</param>
    /// <returns>Trả về <c>true</c> nếu payload hợp lệ, <c>false</c> nếu không.</returns>
    protected static bool TryParsePayload(IPacket packet, int expectedParts, out string[] parts)
    {
        parts = Encoding.UTF8.GetString(packet.Payload.ToArray()).Split('|', StringSplitOptions.TrimEntries);
        return parts.Length >= expectedParts;
    }

    /// <summary>
    /// Chuyển đổi danh sách đối tượng thành mảng byte sử dụng JSON.
    /// </summary>
    /// <typeparam name="T">Kiểu của đối tượng trong danh sách.</typeparam>
    /// <param name="list">Danh sách đối tượng cần chuyển đổi.</param>
    /// <returns>Mảng byte chứa dữ liệu JSON của danh sách.</returns>
    protected static byte[] ToBytes<T>(List<T> list)
        => JsonSerializer.SerializeToUtf8Bytes(list);

    /// <summary>
    /// Giải mã mảng byte thành danh sách đối tượng sử dụng JSON.
    /// </summary>
    /// <typeparam name="T">Kiểu của đối tượng cần giải mã.</typeparam>
    /// <param name="data">Mảng byte chứa dữ liệu JSON.</param>
    /// <returns>Danh sách đối tượng nếu thành công, <c>null</c> nếu thất bại.</returns>
    protected static List<T>? FromBytes<T>(byte[] data)
        => JsonSerializer.Deserialize<List<T>>(data);

    /// <summary>
    /// Tạo gói tin lỗi chứa thông báo lỗi cụ thể.
    /// </summary>
    /// <param name="message">Thông báo lỗi cần gửi.</param>
    /// <returns>Mảng byte chứa gói tin lỗi đã được tuần tự hóa.</returns>
    protected static byte[] CreateErrorPacket(string message)
        => new Packet(PacketFlags.None, PacketPriority.None, (int)Command.Error, message).Serialize();

    /// <summary>
    /// Tạo gói tin thành công chứa thông báo phản hồi.
    /// </summary>
    /// <param name="message">Thông báo thành công cần gửi.</param>
    /// <returns>Mảng byte chứa gói tin thành công đã được tuần tự hóa.</returns>
    protected static byte[] CreateSuccessPacket(string message)
        => new Packet(PacketFlags.None, PacketPriority.None, (int)Command.Success, message).Serialize();

    /// <summary>
    /// Chuyển đổi chuỗi thành số nguyên, nếu không hợp lệ thì trả về giá trị mặc định.
    /// </summary>
    protected static int ParseInt(ReadOnlySpan<char> value, int defaultValue)
        => int.TryParse(value, out int result) ? result : defaultValue;

    /// <summary>
    /// Chuyển đổi chuỗi thành số thực, nếu không hợp lệ thì trả về giá trị mặc định.
    /// </summary>
    protected static double ParseDouble(ReadOnlySpan<char> value, double defaultValue)
        => double.TryParse(value, out double result) ? result : defaultValue;

    /// <summary>
    /// Chuyển đổi chuỗi thành kiểu enum, nếu không hợp lệ thì trả về giá trị mặc định.
    /// </summary>
    protected static T ParseEnum<T>(ReadOnlySpan<char> value, T defaultValue) where T : struct, Enum
        => Enum.TryParse(value, true, out T result) ? result : defaultValue;
}
