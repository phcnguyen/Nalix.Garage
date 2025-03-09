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

    private NetworkStream? _stream;
    private TcpClient? _client; // Nullable để quản lý tốt hơn
    private bool _disposed;

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

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

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

    public async Task SendAsync(IPacket packet, CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
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

    public async Task<IPacket> ReceiveAsync(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();
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

    public void Dispose()
    {
        if (_disposed) return;

        _stream?.Dispose();
        _client?.Dispose();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_stream != null)
            await _stream.DisposeAsync().ConfigureAwait(false);
        _client?.Dispose();
        _disposed = true;
    }

    private void ThrowIfDisposed()
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(SocketClient));
    }

    private void EnsureConnected()
    {
        if (_stream == null || !_client!.Connected)
            throw new InvalidOperationException("Not connected to the server.");
    }

    private async ValueTask CleanupAsync()
    {
        if (_stream != null)
            await _stream.DisposeAsync().ConfigureAwait(false);
        _client?.Close();
    }
}