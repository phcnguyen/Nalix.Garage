using Auto.Common.Enums;
using Auto.Desktop.Helpers;
using Notio.Common.Package;
using Notio.Cryptography.Asymmetric;
using Notio.Cryptography.Hash;
using Notio.Network.Package;
using System;

namespace Auto.Desktop.ViewModels;

public sealed class MainViewModel : BaseViewModel
{
    /// <summary>
    /// Establishes a basic connection to the server at the specified IP and port.
    /// </summary>
    /// <param name="errorMessage">Output parameter containing the error message if the connection fails.</param>
    /// <returns>True if the connection is successful, false otherwise.</returns>
    public static bool EstablishServerConnection(out string errorMessage)
    {
        errorMessage = string.Empty;
        string ip = "192.168.1.3";
        int port = 5000;

        try
        {
            SocketClient.Instance.Connect(ip, port);
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = $"Server connection failed\nIP: {ip}, Port: {port}\n{ex.Message}";
            return false;
        }
    }

    /// <summary>
    /// Initializes a secure connection with the server using X25519 key exchange.
    /// </summary>
    /// <param name="errorMessage">Output parameter containing the error message if the secure connection fails.</param>
    /// <returns>True if the secure connection is established, false otherwise.</returns>
    public static bool EstablishSecureConnection(out string errorMessage)
    {
        try
        {
            // Generate key pair for X25519 encryption
            (byte[] privateKey, byte[] publicKey) = X25519.GenerateKeyPair();

            // Create packet to initiate secure connection
            Packet packet = new(
                PacketType.Binary, PacketFlags.None, PacketPriority.None,
                (ushort)Command.InitiateSecureConnection, publicKey);

            // Send public key and receive server's response
            SocketClient.Instance.Send(packet);
            IPacket packetReceive = SocketClient.Instance.Receive();

            // Validate received packet
            if (packetReceive == null || packetReceive.Payload.Length != 32)
            {
                errorMessage = $"Invalid server response - Expected 32-byte payload." +
                               $"\nReceived: {packetReceive?.Payload.Length ?? 0} bytes.";
                return false;
            }

            // Compute shared secret and set encryption key
            byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packetReceive.Payload.ToArray());
            SocketClient.Instance.EncryptionKey = Sha256.HashData(sharedSecret);

            errorMessage = string.Empty;
            return true;
        }
        catch (Exception ex)
        {
            errorMessage = $"Secure connection failed\n{ex.Message}";
            return false;
        }
    }
}