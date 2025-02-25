using Auto.Server.Network;
using Notio.Logging;
using Notio.Network.Handlers;
using Notio.Network.Package.Helpers;
using Notio.Shared.Memory.Buffer;
using System;

namespace Auto.Server.Threading;

public static class AppConfig
{
    public static ServerListener InitializeServer()
        => new(new ServerProtocol(
            new PacketDispatcher(cfg => cfg.WithLogging(CLogging.Instance)
               .WithPacketSerialization(
                    packet => new Memory<byte>(PackageSerializeHelper.Serialize((Notio.Network.Package.Packet)packet)),
                    data => PackageSerializeHelper.Deserialize(data.Span))
               .WithErrorHandler((exception, command) => CLogging.Instance.Error($"Error handling command:{command}", exception))
               .WithPacketCrypto(null, null)
               .WithPacketCompression(null, null)
               .WithHandler<HandshakeHandler>()
        )), new BufferAllocator(), CLogging.Instance);
}