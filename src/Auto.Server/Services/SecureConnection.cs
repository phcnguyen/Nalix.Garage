using Auto.Common.Enums;
using Notio.Common.Attributes;
using Notio.Common.Authentication;
using Notio.Common.Connection;
using Notio.Common.Data;
using Notio.Cryptography.Asymmetric;
using Notio.Cryptography.Hash;
using Notio.Logging;
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
internal sealed class SecureConnection : Base.BaseService
{
    /// <summary>
    /// Khởi tạo kết nối bảo mật bằng thuật toán X25519.
    /// Định dạng dữ liệu: Binary (32 byte khóa công khai X25519 từ client).
    /// </summary>
    /// <param name="packet">Gói tin chứa khóa công khai X25519 của client.</param>
    /// <param name="connection">Đối tượng kết nối với client.</param>
    [PacketCommand((int)Command.InitiateSecureConnection, Authoritys.Guest)]
    public static void InitiateSecureConnection(IPacket packet, IConnection connection)
    {
        Console.WriteLine(11111111111);
        if (packet.Type != (byte)PacketType.Binary)
        {
            connection.Send(CreateErrorPacket("Unsupported packet type."));
            return;
        }

        if (packet.Payload.Length != 32) // X25519 public key phải là 32 byte
        {
            CLogging.Instance.Warn(
                $"Invalid public key length {packet.Payload.Length} from connection {connection.RemoteEndPoint}");
            connection.Send(CreateErrorPacket("Invalid public key."));
            return;
        }

        try
        {
            // Tạo cặp khóa X25519
            (byte[] privateKey, byte[] publicKey) = X25519.GenerateKeyPair();

            // Tính shared secret
            byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packet.Payload.ToArray());

            // Dùng SHA-256 để sinh key mã hóa
            connection.Metadata["X25519_PrivateKey"] = privateKey;
            connection.EncryptionKey = Sha256.HashData(sharedSecret);

            if (connection.Send(new Packet(
                PacketType.Binary, PacketFlags.None, PacketPriority.None,
                (int)Command.Success, publicKey).Serialize()))
            {
                // Nâng quyền user
                connection.Authority = Authoritys.User;
                CLogging.Instance.Info($"Secure connection initiated successfully for connection {connection.Id}");
            }
            else
            {
                CLogging.Instance.Error($"Failed to send public key response to connection {connection.Id}");
            }
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to initiate secure connection for connection {connection.Id}", ex);
            connection.Send(CreateErrorPacket("Internal error during secure connection initiation."));
        }
    }

    /// <summary>
    /// Hoàn tất quy trình xác thực khóa bảo mật.
    /// Định dạng dữ liệu: Binary (32 byte khóa công khai X25519 từ client).
    /// </summary>
    /// <param name="packet">Gói tin chứa khóa công khai X25519 của client.</param>
    /// <param name="connection">Đối tượng kết nối với client.</param>
    [PacketCommand((int)Command.FinalizeSecureConnection, Authoritys.Guest)]
    public static void FinalizeSecureConnection(IPacket packet, IConnection connection)
    {
        if (packet.Type != (byte)PacketType.Binary)
        {
            connection.Send(CreateErrorPacket("Unsupported packet type."));
            return;
        }
        if (packet.Payload.Length != 32)
        {
            CLogging.Instance.Warn(
                $"Invalid public key length {packet.Payload.Length} from connection {connection.RemoteEndPoint}");
            connection.Send(CreateErrorPacket("Invalid public key."));
            return;
        }

        if (!connection.Metadata.TryGetValue("X25519_PrivateKey", out object? privateKeyObj) ||
            privateKeyObj is not byte[] privateKey)
        {
            CLogging.Instance.Warn($"Missing or invalid X25519 private key for connection {connection.RemoteEndPoint}");
            connection.Send(CreateErrorPacket("Invalid public key."));
            return;
        }

        try
        {
            // Tính lại shared secret để xác thực
            byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packet.Payload.ToArray());
            byte[] derivedKey = Sha256.HashData(sharedSecret);

            // Kiểm tra khóa đã khớp với giá trị trước đó chưa
            if (connection.EncryptionKey.SequenceEqual(derivedKey))
            {
                CLogging.Instance.Info($"Secure connection finalized successfully for connection {connection.RemoteEndPoint}");
                connection.Send(CreateSuccessPacket("Secure connection established."));
            }
            else
            {
                CLogging.Instance.Warn($"Key mismatch during finalization for connection {connection.RemoteEndPoint}");
                connection.Send(CreateErrorPacket("Key mismatch."));
            }
        }
        catch (Exception ex)
        {
            CLogging.Instance.Error($"Failed to finalize secure connection for connection {connection.RemoteEndPoint}", ex);
            connection.Send(CreateErrorPacket("Internal error during secure connection finalization."));
        }
    }
}