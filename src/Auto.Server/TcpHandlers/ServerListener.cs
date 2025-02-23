using Notio.Common.Logging;
using Notio.Common.Memory;
using Notio.Network.Listeners;
using Notio.Network.Protocols;

namespace Auto.Server.TcpHandlers;

public class ServerListener(IProtocol protocol, IBufferPool bufferPool, ILogger logger)
    : Listener(protocol, bufferPool, logger)
{
}