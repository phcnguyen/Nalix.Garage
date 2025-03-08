using Auto.Server.Services;
using Notio.Logging;
using Notio.Network.Handlers;
using Notio.Network.Package;
using Notio.Network.Package.Helpers;
using Notio.Shared.Memory.Buffers;

namespace Auto.Server.Threading;

public static class AppConfig
{
    public static Network.ServerListener InitializeServer()
        => new(new Network.ServerProtocol(
            new PacketDispatcher(cfg => cfg.WithLogging(CLogging.Instance)
               .WithPacketSerialization(
                    packet => new System.Memory<byte>(PackageSerializeHelper.Serialize((Packet)packet)),
                    data => PackageSerializeHelper.Deserialize(data.Span))
               .WithErrorHandler((exception, command) => CLogging.Instance.Error($"Error handling command:{command}", exception))
               .WithPacketCrypto(
                    (packet, connnect) => PacketEncryptionHelper
                        .EncryptPayload((Packet)packet, connnect.EncryptionKey, connnect.Mode),
                    (packet, connnect) => PacketEncryptionHelper
                        .DecryptPayload((Packet)packet, connnect.EncryptionKey, connnect.Mode))
               .WithPacketCompression(null, null)
               .WithHandler<SecureConnection>()
        )), new BufferAllocator(), CLogging.Instance);
}