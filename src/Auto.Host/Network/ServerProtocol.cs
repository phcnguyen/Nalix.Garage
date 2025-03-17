using Notio.Common.Connection;
using Notio.Logging;
using Notio.Network.Handlers;
using Notio.Network.Protocols;
using System;

namespace Auto.Host.Network;

public sealed class ServerProtocol(IPacketDispatcher packetDispatcher) : Protocol
{
    private readonly IPacketDispatcher _packetDispatcher = packetDispatcher;
    public override bool KeepConnectionOpen => true;
    public override void ProcessMessage(object sender, IConnectEventArgs args)
    {
        try
        {
            CLogging.Instance.Info($"[ProcessMessage] Received packet from {args.Connection.RemoteEndPoint}");
            _packetDispatcher.HandlePacket(args.Connection.IncomingPacket, args.Connection);
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"[ProcessMessage] Error: {ex}");
            args.Connection.Disconnect();
        }
    }
}