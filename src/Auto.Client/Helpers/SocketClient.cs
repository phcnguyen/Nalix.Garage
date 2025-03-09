using Notio.Common.Cryptography;
using Notio.Common.Package;
using Notio.Network.Package;
using Notio.Network.Package.Extensions;
using System.Buffers;
using System.IO;
using System.Net.Sockets;

namespace Auto.Client.Helpers;

/// <summary>
/// Helper để thực hiện kết nối TCP với timeout và xử lý lỗi tốt hơn
/// </summary>
public sealed class SocketClient : IDisposable
{
    private readonly int _port;
    private readonly string _server;
    private readonly int _connectionTimeout;

    private bool _disposed;
    private TcpClient? _client;
    private NetworkStream? _stream;

    /// <summary>
    /// Gets the encryption key used for securing communication.
    /// </summary>
    public byte[] EncryptionKey = [];

    /// <summary>
    /// Gets or sets the encryption mode used.
    /// </summary>
    public EncryptionMode Mode { get; set; }

    /// <summary>
    /// Khởi tạo một SocketClient mới
    /// </summary>
    /// <param name="server">Địa chỉ server</param>
    /// <param name="port">Cổng kết nối</param>
    /// <param name="connectionTimeoutMs">Thời gian timeout kết nối (ms), mặc định 30000ms</param>
    /// <exception cref="ArgumentNullException">Khi server là null</exception>
    /// <exception cref="ArgumentOutOfRangeException">Khi port không nằm trong khoảng 1-65535</exception>
    public SocketClient(string server, int port, int connectionTimeoutMs = 30000)
    {
        _server = server ?? throw new ArgumentNullException(nameof(server));
        if (port is <= 0 or > 65535)
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");

        _port = port;
        _connectionTimeout = connectionTimeoutMs;
        _client = new TcpClient
        {
            NoDelay = true // Tắt Nagle algorithm để cải thiện latency
        };
    }

    #region Synchronous Methods

    /// <summary>
    /// Thực hiện kết nối đồng bộ tới server
    /// </summary>
    /// <exception cref="ObjectDisposedException">Khi object đã bị dispose</exception>
    /// <exception cref="IOException">Khi có lỗi kết nối</exception>
    public void Connect()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SocketClient));

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
        ObjectDisposedException.ThrowIf(_disposed, nameof(SocketClient));
        EnsureConnected();

        try
        {
            if (packet is not Packet concretePacket)
                throw new ArgumentException("Invalid packet type.", nameof(packet));

            ReadOnlyMemory<byte> data = concretePacket.Serialize();
            _stream!.Write(data.Span);
            _stream.Flush();
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
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
        ObjectDisposedException.ThrowIf(_disposed, nameof(SocketClient));
        EnsureConnected();

        try
        {
            byte[] buffer = ArrayPool<byte>.Shared.Rent(2);
            try
            {
                _stream!.ReadExactly(buffer, 0, 2);
                ushort packetLength = BitConverter.ToUInt16(buffer, 0);

                byte[] packetBuffer = ArrayPool<byte>.Shared.Rent(2 + packetLength);
                try
                {
                    buffer.AsSpan(0, 2).CopyTo(packetBuffer);
                    _stream.ReadExactly(packetBuffer, 2, packetLength);
                    return packetBuffer.Deserialize();
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
    /// Đóng kết nối đồng bộ
    /// </summary>
    public void Close()
    {
        if (_disposed || _client == null) return;

        try
        {
            _stream?.Dispose();
            _client.Close();
        }
        finally
        {
            _stream = null;
            _client = null;
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
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SocketClient));

        if (_client?.Connected == true)
        {
            await CleanupAsync();
            _client = new TcpClient { NoDelay = true };
        }

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(_connectionTimeout);

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
                $"Connection to {_server}:{_port} timed out after {_connectionTimeout}ms");
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
        ObjectDisposedException.ThrowIf(_disposed, nameof(SocketClient));
        EnsureConnected();

        try
        {
            if (packet is not Packet concretePacket)
                throw new ArgumentException("Invalid packet type.", nameof(packet));

            ReadOnlyMemory<byte> data = concretePacket.Serialize();
            await _stream!.WriteAsync(data, cancellationToken).ConfigureAwait(false);
            await _stream.FlushAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
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
        ObjectDisposedException.ThrowIf(_disposed, nameof(SocketClient));
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
                        packetBuffer.AsMemory(2, packetLength),
                        cancellationToken).ConfigureAwait(false);

                    return packetBuffer.Deserialize();
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
    public void Dispose()
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