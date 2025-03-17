using Notio.Common.Logging;
using Notio.Common.Memory;
using Notio.Network.Listeners;
using Notio.Network.Protocols;

namespace Auto.Host.Network;

public sealed class ServerListener(IProtocol protocol, IBufferPool bufferPool, ILogger logger)
    : Listener(protocol, bufferPool, logger)
{
}