using Auto.Common.Enums;
using Auto.Desktop.Helpers;
using Notio.Common.Package;
using Notio.Cryptography.Asymmetric;
using Notio.Cryptography.Hash;
using Notio.Network.Package;
using System;
using System.Threading.Tasks;

namespace Auto.Desktop.ViewModels;

public sealed class MainViewModel : BaseViewModel
{
    /// <summary>
    /// Establishes a basic connection to the server at the specified IP and port.
    /// </summary>
    /// <returns>True if the connection is successful, false otherwise.</returns>
    public static async Task<(bool Status, string Message)> EstablishServerConnectionAsync()
    {
        string ip = "192.168.1.3";
        int port = 5000;

        try
        {
            await SocketClient.Instance.ConnectAsync(ip, port); // Chuyển sang async
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Server connection failed\nIP: {ip}, Port: {port}\n{ex.Message}");
        }
    }

    /// <summary>
    /// Initializes a secure connection with the server using X25519 key exchange.
    /// </summary>
    /// <returns>True if the secure connection is established, false otherwise.</returns>
    public static async Task<(bool Status, string Message)> EstablishSecureConnectionAsync()
    {
        try
        {
            // Tạo key pair cho X25519
            (byte[] privateKey, byte[] publicKey) = X25519.GenerateKeyPair();

            // Gửi public key cho server
            Packet packet = new(
                PacketType.Binary, PacketFlags.None, PacketPriority.None,
                (ushort)Command.InitiateSecureConnection, publicKey);

            await SocketClient.Instance.SendAsync(packet);
            IPacket packetReceive = await SocketClient.Instance.ReceiveAsync();

            // Kiểm tra phản hồi từ server
            if (packetReceive == null || packetReceive.Payload.Length != 32)
            {
                return (false, $"Invalid server response - Expected 32-byte payload." +
                               $"\nReceived: {packetReceive?.Payload.Length ?? 0} bytes.");
            }

            // Tính toán shared secret và đặt khóa mã hóa
            byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packetReceive.Payload.ToArray());
            SocketClient.Instance.EncryptionKey = Sha256.HashData(sharedSecret);

            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            return (false, $"Secure connection failed\n{ex.Message}");
        }
    }
}