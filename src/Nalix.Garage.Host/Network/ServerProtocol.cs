﻿using Nalix.Common.Connection;
using Nalix.Logging;
using Nalix.Network.Dispatch;
using Nalix.Network.Package;
using Nalix.Network.Protocols;
using System;
using System.Threading;

namespace Nalix.Garage.Host.Network;

public sealed class ServerProtocol(IPacketDispatch<Packet> packetDispatcher) : Protocol
{
    private readonly IPacketDispatch<Packet> _packetDispatcher = packetDispatcher;

    public override bool KeepConnectionOpen => true;

    public override void OnAccept(IConnection connection, CancellationToken cancellationToken = default)
    {
        base.OnAccept(connection, cancellationToken);
        NLogix.Host.Instance.Info($"[OnAccept] Connection accepted from {connection.RemoteEndPoint}");
    }

    public override void ProcessMessage(object sender, IConnectEventArgs args)
    {
        try
        {
            NLogix.Host.Instance.Info($"[ProcessMessage] Received packet from {args.Connection.RemoteEndPoint}");
            _packetDispatcher.HandlePacket(args.Connection.IncomingPacket, args.Connection);
            NLogix.Host.Instance.Info($"[ProcessMessage] Successfully processed packet from {args.Connection.RemoteEndPoint}");
        }
        catch (Exception ex)
        {
            NLogix.Host.Instance.Error($"[ProcessMessage] Error processing packet from {args.Connection.RemoteEndPoint}: {ex}");
            args.Connection.Disconnect();
        }
    }

    protected override void OnConnectionError(IConnection connection, Exception exception)
    {
        base.OnConnectionError(connection, exception);
        NLogix.Host.Instance.Error($"[OnConnectionError] Connection error with {connection.RemoteEndPoint}: {exception}");
    }

    protected override void OnDisposing()
    {
        NLogix.Host.Instance.Info("[OnDisposing] ServerProtocol is shutting down.");
        base.OnDisposing();
    }
}