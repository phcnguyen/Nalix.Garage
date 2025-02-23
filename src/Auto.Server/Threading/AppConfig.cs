using Auto.Server.TcpHandlers;
using Notio.Logging;
using Notio.Network.Handlers;
using Notio.Network.Package.Helpers;
using Notio.Shared.Memory.Buffer;

namespace Auto.Server.Threading;

public static class AppConfig
{
    public static ServerListener InitializeTcpServer()
    {
        BufferAllocator bufferAllocator = new();

        PacketDispatcher dispatcher = new(cfg =>
            cfg.WithPacketSerialization(
                    packet => new Memory<byte>(PackageSerializeHelper.Serialize((Notio.Network.Package.Packet)packet)),
                    data => PackageSerializeHelper.Deserialize(data.Span))
               .WithErrorHandler((exception, command) => CLogging.Instance.Error($"Error handling command:{command}", exception))
               .WithHandler<PacketHandler>()
               .WithPacketCrypto(null, null)
               .WithPacketCompression(null, null)
        );

        ServerProtocol serverProtocol = new(dispatcher);
        return new ServerListener(serverProtocol, bufferAllocator, CLogging.Instance);
    }
}