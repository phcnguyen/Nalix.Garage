using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Cryptography.Asymmetric;
using Notio.Cryptography.Hash;
using Notio.Network.Handlers;
using Notio.Network.Package;
using Notio.Network.Package.Enums;
using Notio.Network.Package.Extensions;

namespace Auto.Server.TcpHandlers;

[PacketController]
internal class PacketHandler
{
    [PacketCommand(1, Authoritys.Guests)]
    public static void InitiateSecureConnection(IConnection connection, Packet packet)
    {
        byte[] _x25519PublicKey;
        byte[] _x25519PrivateKey;
        (_x25519PrivateKey, _x25519PublicKey) = X25519.GenerateKeyPair();

        byte[] sharedSecret = X25519.ComputeSharedSecret(_x25519PrivateKey, packet.Payload.ToArray());

        // Derive encryption key from shared secret (e.g., using SHA-256)
        connection.EncryptionKey = Sha256.HashData(sharedSecret);

        connection.Send(new Packet(PacketType.Binary, PacketFlags.None, PacketPriority.None, 0, _x25519PublicKey).Serialize());
    }
}