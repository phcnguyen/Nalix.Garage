using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Network.Package.Enums;
using Notio.Network.Package.Extensions;
using System;
using System.Text;

namespace Auto.Server.Services;

public abstract class BaseService
{
    /// <summary>
    /// Một mảng byte tĩnh chứa gói dữ liệu mặc định đã được tuần tự hóa.
    /// Gói này có kiểu Binary, không có cờ hoặc mức độ ưu tiên, và không chứa dữ liệu payload.
    /// </summary>
    protected static readonly byte[] PacketDefault = new Packet(
        PacketType.Binary, PacketFlags.None, PacketPriority.None, 0, Array.Empty<byte>()).Serialize();

    /// <summary>
    /// Đây là một phương thức kiểm tra xem dữ liệu payload có hợp lệ không và chia nó thành một mảng các chuỗi.
    /// Nếu số phần tử trong mảng lớn hơn hoặc bằng số lượng mong đợi, trả về true; nếu không, trả về false.
    /// </summary>
    /// <param name="packet">Gói dữ liệu mạng chứa payload cần kiểm tra</param>
    /// <param name="expectedParts">Số lượng phần tử tối thiểu mong đợi sau khi tách</param>
    /// <param name="parts">Mảng chuỗi kết quả sau khi tách payload</param>
    /// <returns>Trả về true nếu payload hợp lệ, false nếu không</returns>
    protected static bool TryParsePayload(IPacket packet, int expectedParts, out string[] parts)
    {
        parts = Encoding.UTF8.GetString(packet.Payload.ToArray()).Split('|', StringSplitOptions.TrimEntries);
        return parts.Length >= expectedParts;
    }
}
