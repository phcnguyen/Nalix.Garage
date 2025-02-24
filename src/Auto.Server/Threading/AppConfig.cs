using Auto.Server.TcpHandlers;
using Notio.Logging;
using Notio.Network.Handlers;
using Notio.Network.Package.Helpers;
using Notio.Shared.Memory.Buffer;
using System;

namespace Auto.Server.Threading;

public static class AppConfig
{
    public static ServerListener InitializeTcpServer()
    {
        PacketDispatcher dispatcher = new(cfg =>
            cfg.WithLogging(CLogging.Instance)
               .WithPacketSerialization(
                    packet => new Memory<byte>(PackageSerializeHelper.Serialize((Notio.Network.Package.Packet)packet)),
                    data => PackageSerializeHelper.Deserialize(data.Span))
               .WithErrorHandler((exception, command) => CLogging.Instance.Error($"Error handling command:{command}", exception))
               .WithPacketCrypto(null, null)
               .WithPacketCompression(null, null)
               .WithHandler<PacketHandler>()
        );

        return new ServerListener(new ServerProtocol(dispatcher), new BufferAllocator(), CLogging.Instance);
    }
}