using Auto.Common.Enums;
using Notio.Common.Package;
using Notio.Common.Security;
using Notio.Cryptography.Asymmetric;
using Notio.Cryptography.Hash;
using Notio.Network.Package;
using Notio.Network.Package.Extensions;
using Notio.Shared.Injection;
using System;
using System.Buffers;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Desktop.Sockets;

/// <summary>
/// Helper để thực hiện kết nối TCP với timeout và xử lý lỗi tốt hơn
/// </summary>
public sealed class NetworkClient : SingletonBase<NetworkClient>, IDisposable, IAsyncDisposable
{
    private int _port = 7777;
    private string _server = "127.0.0.1";

    private bool _disposed;
    private TcpClient? _client;
    private NetworkStream? _stream;

    /// <summary>
    /// Gets or sets the encryption mode used.
    /// </summary>
    public EncryptionMode Mode { get; set; }

    /// <summary>
    /// Gets the encryption key used for securing communication.
    /// </summary>
    public byte[] EncryptionKey { get; private set; } = [];

    /// <summary>
    /// Check that the client is connected.
    /// </summary>
    public bool IsConnected => _client?.Connected == true && _stream != null;

    /// <summary>
    /// Khởi tạo một NetworkClient mới
    /// </summary>
    /// <exception cref="ArgumentNullException">Khi server là null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Khi port không nằm trong khoảng 1-65535</exception>
    private NetworkClient()
    {
        _client = new TcpClient
        {
            NoDelay = true
        };
    }

    /// <summary>
    /// Sets the server address and port for the connection.
    /// </summary>
    /// <param name="server">The server IP or hostname. If null or empty, the current value remains unchanged.</param>
    /// <param name="port">The port number. Must be between 1 and 65535. If null, the current value remains unchanged.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="port"/> is outside the valid range.</exception>
    public void Set(string? server, int? port)
    {
        if (!string.IsNullOrWhiteSpace(server))
            _server = server;

        if (port.HasValue)
        {
            if (port.Value is <= 0 or > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");

            _port = port.Value;
        }
    }

    #region Secure Handshake

    /// <summary>
    /// Initializes a secure connection with the server using X25519 key exchange.
    /// </summary>
    /// <returns>True if the secure connection is established, false otherwise.</returns>
    public async Task<(bool Status, string Message)> SecureHandshakeAsync()
    {
        if (!Instance.IsConnected)
        {
            try
            {
                await Instance.ConnectAsync(_server, _port);
            }
            catch (Exception ex)
            {
                return (false, $"Server connection failed\nIP: {_server}, Port: {_port}\n{ex.Message}");
            }
        }

        try
        {
            // Tạo key pair cho X25519
            (byte[] privateKey, byte[] publicKey) = X25519.GenerateKeyPair();

            // Gửi public key cho server
            Packet packet = new(
                PacketType.Binary, PacketFlags.None, PacketPriority.None,
                (ushort)Command.InitiateSecureConnection, publicKey
            );

            await Instance.SendAsync(packet);
            IPacket packetReceive = await Instance.ReceiveAsync();

            // Kiểm tra phản hồi từ server
            if (packetReceive == null || packetReceive.Payload.Length != 32)
            {
                return (false, $"Invalid server response - Expected 32-byte payload." +
                               $"\nReceived: {packetReceive?.Payload.Length ?? 0} bytes.");
            }

            // Tính toán shared secret và đặt khóa mã hóa
            byte[] sharedSecret = X25519.ComputeSharedSecret(privateKey, packetReceive.Payload.ToArray());
            Instance.EncryptionKey = Sha256.HashData(sharedSecret);

            return (true, "Secure connection established successfully.");
        }
        catch (Exception ex)
        {
            return (false, $"Secure connection failed\n{ex.Message}");
        }
    }

    #endregion

    #region Synchronous Methods

    /// <summary>
    /// Thực hiện kết nối đồng bộ tới server
    /// </summary>
    /// <exception cref="ObjectDisposedException">Khi object đã bị dispose</exception>
    /// <exception cref="IOException">Khi có lỗi kết nối</exception>
    public void Connect(string? server, int? port)
    {
        if (!string.IsNullOrWhiteSpace(server))
            _server = server;

        if (port.HasValue)
        {
            if (port.Value is <= 0 or > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");

            _port = port.Value;
        }

        ObjectDisposedException.ThrowIf(_disposed, nameof(NetworkClient));

        if (_client?.Connected == true)
        {
            Cleanup();
            _client = new TcpClient { NoDelay = true };
        }

        try
        {
            (_client ??= new TcpClient { NoDelay = true })
                .Connect(_server, _port);
            _stream = _client.GetStream();
        }
        catch (SocketException ex)
        {
            throw new IOException($"Error connecting to {_server}:{_port}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Gửi packet đồng bộ qua kết nối
    /// </summary>
    /// <param name="packet">Packet cần gửi</param>
    /// <exception cref="ObjectDisposedException">Khi object đã bị dispose</exception>
    /// <exception cref="InvalidOperationException">Khi chưa kết nối</exception>
    /// <exception cref="ArgumentException">Khi packet không hợp lệ</exception>
    /// <exception cref="IOException">Khi có lỗi gửi dữ liệu</exception>
    public void Send(IPacket packet)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(NetworkClient));
        EnsureConnected();

        try
        {
            if (packet is not Packet concretePacket)
                throw new ArgumentException("Invalid packet type.", nameof(packet));

            Console.WriteLine($"Sending packet: {concretePacket}");
            _stream!.Write(concretePacket.Serialize());
            _stream.Flush();
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
            Console.WriteLine($"[ERROR] Failed to send data: {ex.Message}");
            throw new IOException($"Error sending data to {_server}:{_port}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Nhận packet đồng bộ từ server
    /// </summary>
    /// <returns>Packet nhận được</returns>
    /// <exception cref="ObjectDisposedException">Khi object đã bị dispose</exception>
    /// <exception cref="InvalidOperationException">Khi chưa kết nối</exception>
    /// <exception cref="IOException">Khi có lỗi nhận dữ liệu</exception>
    public IPacket Receive()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(NetworkClient));
        EnsureConnected();

        try
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(2);
            try
            {
                _stream!.ReadExactly(buffer, 0, 2);
                ushort packetLength = BitConverter.ToUInt16(buffer, 0);

                Console.WriteLine($"Receiving packet of length {packetLength} bytes...");

                byte[] packetBuffer = ArrayPool<byte>.Shared.Rent(2 + packetLength);
                try
                {
                    buffer.AsSpan(0, 2).CopyTo(packetBuffer);
                    _stream.ReadExactly(packetBuffer, 2, packetLength - 2);

                    IPacket packet = packetBuffer.Deserialize();

                    Console.WriteLine($"Received packet: {packet}");

                    return packet;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(packetBuffer);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
            Console.WriteLine($"[ERROR] Failed to receive data: {ex.Message}");
            throw new IOException($"Error receiving data from {_server}:{_port}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đóng kết nối đồng bộ
    /// </summary>
    public void Close()
    {
        if (_disposed || _client == null) return;

        Console.WriteLine($"Closing connection to {_server}:{_port}...");
        try
        {
            _stream?.Dispose();
            _client.Close();
        }
        finally
        {
            _stream = null;
            _client = null;
            Console.WriteLine("Connection closed.");
        }
    }

    #endregion

    #region Asynchronous Methods

    /// <summary>
    /// Thực hiện kết nối bất đồng bộ tới server
    /// </summary>
    /// <param name="cancellationToken">Token để hủy thao tác</param>
    /// <returns>Task đại diện cho thao tác kết nối</returns>
    /// <exception cref="ObjectDisposedException">Khi object đã bị dispose</exception>
    /// <exception cref="TimeoutException">Khi kết nối timeout</exception>
    public async Task ConnectAsync(
        string? server, int? port, int timeout = 30000,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(server))
            _server = server;

        if (port.HasValue)
        {
            if (port.Value is <= 0 or > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");

            _port = port.Value;
        }

        ObjectDisposedException.ThrowIf(_disposed, nameof(NetworkClient));

        if (_client?.Connected == true)
        {
            await CleanupAsync();
            _client = new TcpClient { NoDelay = true };
        }

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(timeout);

        try
        {
            await (_client ??= new TcpClient { NoDelay = true })
                .ConnectAsync(_server, _port, cts.Token)
                .ConfigureAwait(false);
            _stream = _client.GetStream();
        }
        catch (OperationCanceledException)
        {
            throw new TimeoutException(
                $"Connection to {_server}:{_port} timed out after {timeout}ms");
        }
    }

    /// <summary>
    /// Gửi packet bất đồng bộ qua kết nối
    /// </summary>
    /// <param name="packet">Packet cần gửi</param>
    /// <param name="cancellationToken">Token để hủy thao tác</param>
    /// <returns>Task đại diện cho thao tác gửi</returns>
    /// <exception cref="ObjectDisposedException">Khi object đã bị dispose</exception>
    /// <exception cref="InvalidOperationException">Khi chưa kết nối</exception>
    /// <exception cref="ArgumentException">Khi packet không hợp lệ</exception>
    /// <exception cref="IOException">Khi có lỗi gửi dữ liệu</exception>
    public async Task SendAsync(IPacket packet, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(NetworkClient));
        EnsureConnected();

        try
        {
            if (packet is not Packet concretePacket)
                throw new ArgumentException("Invalid packet type.", nameof(packet));

            Console.WriteLine($"Sending packet: {concretePacket}");
            await _stream!.WriteAsync(concretePacket.Serialize(), cancellationToken).ConfigureAwait(false);
            await _stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
            Console.WriteLine($"[ERROR] Failed to send data: {ex.Message}");
            throw new IOException($"Error sending data to {_server}:{_port}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Nhận packet bất đồng bộ từ server
    /// </summary>
    /// <param name="cancellationToken">Token để hủy thao tác</param>
    /// <returns>Task chứa packet nhận được</returns>
    /// <exception cref="ObjectDisposedException">Khi object đã bị dispose</exception>
    /// <exception cref="InvalidOperationException">Khi chưa kết nối</exception>
    /// <exception cref="IOException">Khi có lỗi nhận dữ liệu</exception>
    public async Task<IPacket> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(NetworkClient));
        EnsureConnected();

        try
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(2);
            try
            {
                await _stream!.ReadExactlyAsync(buffer.AsMemory(0, 2), cancellationToken)
                    .ConfigureAwait(false);
                ushort packetLength = BitConverter.ToUInt16(buffer, 0);

                byte[] packetBuffer = ArrayPool<byte>.Shared.Rent(2 + packetLength);
                try
                {
                    buffer.AsSpan(0, 2).CopyTo(packetBuffer);
                    await _stream.ReadExactlyAsync(
                        packetBuffer.AsMemory(2, packetLength - 2),
                        cancellationToken).ConfigureAwait(false);

                    Packet packet = packetBuffer.Deserialize();
                    Console.WriteLine($"Received packet: {packet}");

                    return packet;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(packetBuffer);
                }
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
            throw new IOException($"Error receiving data from {_server}:{_port}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đóng kết nối bất đồng bộ
    /// </summary>
    /// <returns>ValueTask đại diện cho thao tác đóng</returns>
    public async ValueTask CloseAsync()
    {
        if (_disposed || _client == null) return;

        try
        {
            if (_stream != null)
                await _stream.DisposeAsync().ConfigureAwait(false);
            _client.Close();
        }
        finally
        {
            _stream = null;
            _client = null;
        }
    }

    #endregion

    #region Dispose Pattern

    /// <summary>
    /// Giải phóng tài nguyên đồng bộ
    /// </summary>
    public new void Dispose()
    {
        if (_disposed) return;

        _stream?.Dispose();
        _client?.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Giải phóng tài nguyên bất đồng bộ
    /// </summary>
    /// <returns>ValueTask đại diện cho thao tác dispose</returns>
    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_stream != null)
            await _stream.DisposeAsync().ConfigureAwait(false);
        _client?.Dispose();
        _disposed = true;
    }

    #endregion

    #region Private Helpers

    /// <summary>
    /// Kiểm tra trạng thái kết nối
    /// </summary>
    /// <exception cref="InvalidOperationException">Khi chưa kết nối tới server</exception>
    private void EnsureConnected()
    {
        if (_stream == null || !_client!.Connected)
            throw new InvalidOperationException("Not connected to the server.");
    }

    /// <summary>
    /// Dọn dẹp tài nguyên đồng bộ
    /// </summary>
    private void Cleanup()
    {
        _stream?.Dispose();
        _client?.Close();
    }

    /// <summary>
    /// Dọn dẹp tài nguyên bất đồng bộ
    /// </summary>
    /// <returns>ValueTask đại diện cho thao tác dọn dẹp</returns>
    private async ValueTask CleanupAsync()
    {
        if (_stream != null)
            await _stream.DisposeAsync().ConfigureAwait(false);
        _client?.Close();
    }

    #endregion
}