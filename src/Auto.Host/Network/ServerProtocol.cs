using Notio.Common.Connection;
using Notio.Logging;
using Notio.Network.PacketProcessing;
using Notio.Network.Protocols;
using System;
using System.Threading;

namespace Auto.Host.Network;

public sealed class ServerProtocol(IPacketDispatcher packetDispatcher) : Protocol
{
    private readonly IPacketDispatcher _packetDispatcher = packetDispatcher;

    public override bool KeepConnectionOpen => true;

    public override void OnAccept(IConnection connection, CancellationToken cancellationToken = default)
    {
        base.OnAccept(connection, cancellationToken);
        CLogging.Instance.Info($"[OnAccept] Connection accepted from {connection.RemoteEndPoint}");
    }

    public override void ProcessMessage(object sender, IConnectEventArgs args)
    {
        try
        {
            CLogging.Instance.Info($"[ProcessMessage] Received packet from {args.Connection.RemoteEndPoint}");
            _packetDispatcher.HandlePacket(args.Connection.IncomingPacket, args.Connection);
            CLogging.Instance.Info($"[ProcessMessage] Successfully processed packet from {args.Connection.RemoteEndPoint}");
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"[ProcessMessage] Error processing packet from {args.Connection.RemoteEndPoint}: {ex}");
            args.Connection.Disconnect();
        }
    }

    protected override void OnConnectionError(IConnection connection, Exception exception)
    {
        base.OnConnectionError(connection, exception);
        CLogging.Instance.Error($"[OnConnectionError] Connection error with {connection.RemoteEndPoint}: {exception}");
    }

    protected override void OnDisposing()
    {
        CLogging.Instance.Info("[OnDisposing] ServerProtocol is shutting down.");
        base.OnDisposing();
    }
}
