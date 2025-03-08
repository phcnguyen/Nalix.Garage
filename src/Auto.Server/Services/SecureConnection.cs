using Auto.Common.Enums;
using Notio.Common.Connection;
using Notio.Common.Models;
using Notio.Common.Package;
using Notio.Cryptography.Asymmetric;
using Notio.Cryptography.Hash;
using Notio.Network.Handlers;
using Notio.Network.Package;
using Notio.Network.Package.Enums;
using Notio.Network.Package.Extensions;
using System;
using System.Linq;

namespace Auto.Server.Services;

[PacketController]
internal sealed class SecureConnection : BaseService
{
    [PacketCommand((int)Command.InitiateSecureConnection, Authoritys.Guests)]
    public static void InitiateSecureConnection(IPacket packet, IConnection connection)
    {
        if (packet.Payload.Length != 32)  // X25519 public key phải có đúng 32 byte
        {
            connection.Send(PacketDefault);
            return;
        }

        (byte[] privateKey, byte[] publicKey) = X25519.GenerateKeyPair();

        connection.Metadata["X25519_PrivateKey"] = privateKey;

        if (connection.Send(new
            Packet(PacketType.Binary, PacketFlags.None, PacketPriority.None, 0, publicKey).Serialize()))
        {
            // Tính shared secret
            byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packet.Payload.ToArray());

            // Dùng SHA-256 để sinh key mã hóa
            connection.EncryptionKey = Sha256.HashData(sharedSecret);

            // Nâng quyền user
            connection.Authority = Authoritys.User;
        }
    }

    [PacketCommand((int)Command.FinalizeSecureConnection, Authoritys.Guests)]
    public static void FinalizeSecureConnection(IPacket packet, IConnection connection)
    {
        if (!connection.Metadata.TryGetValue("X25519_PrivateKey", out object? privateKeyObj) || privateKeyObj is not byte[] privateKey)
        {
            connection.Send(PacketDefault);
            return;
        }

        if (packet.Payload.Length != 32)
        {
            connection.Send(PacketDefault);
            return;
        }

        // Tính lại shared secret để xác thực
        byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packet.Payload.ToArray());
        byte[] derivedKey = Sha256.HashData(sharedSecret);

        // Kiểm tra khóa đã khớp với giá trị trước đó chưa
        if (connection.EncryptionKey.SequenceEqual(derivedKey))
        {
            connection.Send(
                new Packet(PacketType.Binary, PacketFlags.None, PacketPriority.None, 0, new byte[] { 1 }).Serialize());
        }
        else
        {
            connection.Send(PacketDefault);
        }
    }
}