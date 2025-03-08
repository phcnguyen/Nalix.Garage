using Notio.Common.Connection;
using Notio.Network.Handlers;
using Notio.Network.Protocols;

namespace Auto.Server.Network;

public sealed class ServerProtocol(IPacketDispatcher packetDispatcher) : Protocol
{
    private readonly IPacketDispatcher _packetDispatcher = packetDispatcher;

    public new bool KeepConnectionOpen = true;

    public override void ProcessMessage(object sender, IConnectEventArgs args)
        => _packetDispatcher.HandlePacket(args.Connection.IncomingPacket, args.Connection);
}