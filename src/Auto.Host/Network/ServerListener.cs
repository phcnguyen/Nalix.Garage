using Notio.Common.Caching;
using Notio.Common.Logging;
using Notio.Network.Core;
using Notio.Network.Listeners;

namespace Auto.Host.Network;

public sealed class ServerListener(IProtocol protocol, IBufferPool bufferPool, ILogger logger)
    : Listener(protocol, bufferPool, logger)
{
}