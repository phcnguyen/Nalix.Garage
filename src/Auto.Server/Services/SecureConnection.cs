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

/// <summary>
/// Dịch vụ xử lý kết nối bảo mật bằng X25519 và SHA-256.
/// </summary>
[PacketController]
internal sealed class SecureConnection : BaseService
{
    /// <summary>
    /// Khởi tạo kết nối bảo mật bằng thuật toán X25519.
    /// </summary>
    /// <param name="packet">Gói tin chứa khóa công khai X25519 của client.</param>
    /// <param name="connection">Đối tượng kết nối với client.</param>
    [PacketCommand((int)Command.InitiateSecureConnection, Authoritys.Guests)]
    public static void InitiateSecureConnection(IPacket packet, IConnection connection)
    {
        if (packet.Payload.Length != 32)  // X25519 public key phải có đúng 32 byte
        {
            connection.Send(CreateErrorPacket("Invalid public key."));
            return;
        }

        // Tạo cặp khóa X25519
        (byte[] privateKey, byte[] publicKey) = X25519.GenerateKeyPair();

        // Tính shared secret
        byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packet.Payload.ToArray());

        // Dùng SHA-256 để sinh key mã hóa
        connection.Metadata["X25519_PrivateKey"] = privateKey;
        connection.EncryptionKey = Sha256.HashData(sharedSecret);

        if (connection.Send(new
            Packet(
            PacketType.Binary, PacketFlags.None, PacketPriority.None,
            (int)Command.Success, publicKey).Serialize()))
        {
            // Nâng quyền user
            connection.Authority = Authoritys.User;
        }
    }

    /// <summary>
    /// Hoàn tất quy trình xác thực khóa bảo mật.
    /// </summary>
    /// <param name="packet">Gói tin chứa khóa công khai X25519 của client.</param>
    /// <param name="connection">Đối tượng kết nối với client.</param>
    [PacketCommand((int)Command.FinalizeSecureConnection, Authoritys.Guests)]
    public static void FinalizeSecureConnection(IPacket packet, IConnection connection)
    {
        if (packet.Payload.Length != 32)
        {
            connection.Send(CreateErrorPacket("Invalid public key."));
            return;
        }

        if (!connection.Metadata.TryGetValue("X25519_PrivateKey",
            out object? privateKeyObj) || privateKeyObj is not byte[] privateKey)
        {
            connection.Send(CreateErrorPacket("Invalid public key."));
            return;
        }

        // Tính lại shared secret để xác thực
        byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packet.Payload.ToArray());
        byte[] derivedKey = Sha256.HashData(sharedSecret);

        // Kiểm tra khóa đã khớp với giá trị trước đó chưa
        if (connection.EncryptionKey.SequenceEqual(derivedKey))
            connection.Send(CreateSuccessPacket("Secure connection established."));
        else
            connection.Send(CreateErrorPacket("Key mismatch."));
    }
}
