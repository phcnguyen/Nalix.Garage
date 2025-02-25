using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Auto.Common.Helpers;

/// <summary>
/// Helper để thực hiện kết nối TCP với timeout và xử lý lỗi tốt hơn
/// </summary>
public class TcpClientHelper : IDisposable
{
    private readonly string _server;
    private readonly int _port;
    private readonly int _connectionTimeout;
    private TcpClient _client;
    private NetworkStream _stream;
    private bool _disposed;

    /// <summary>
    /// Khởi tạo TcpClientHelper với timeout mặc định là 30 giây
    /// </summary>
    public TcpClientHelper(string server, int port, int connectionTimeoutMs = 30000)
    {
        _server = server ?? throw new ArgumentNullException(nameof(server));
        if (port <= 0 || port > 65535)
            throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535");

        _port = port;
        _connectionTimeout = connectionTimeoutMs;
        _client = new TcpClient();
    }

    /// <summary>
    /// Kết nối đến server với timeout
    /// </summary>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(TcpClientHelper));

        // Đóng kết nối cũ nếu đã mở
        if (_client.Connected)
        {
            _stream?.Close();
            _client.Close();
            _client = new TcpClient();
        }

        using var timeoutCts = new CancellationTokenSource(_connectionTimeout);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);

        try
        {
            await _client.ConnectAsync(_server, _port, linkedCts.Token);
            _stream = _client.GetStream();
        }
        catch (OperationCanceledException) when (timeoutCts.IsCancellationRequested)
        {
            throw new TimeoutException($"Connection to {_server}:{_port} timed out after {_connectionTimeout}ms");
        }
    }

    /// <summary>
    /// Gửi dữ liệu với kiểm soát timeout
    /// </summary>
    public async Task SendAsync(byte[] data, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(TcpClientHelper));

        if (_stream == null)
            throw new InvalidOperationException("Not connected to the server.");

        if (data == null || data.Length == 0)
            throw new ArgumentException("Data to send cannot be null or empty", nameof(data));

        try
        {
            await _stream.WriteAsync(data, cancellationToken);
            await _stream.FlushAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
            throw new IOException($"Error sending data to {_server}:{_port}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Nhận dữ liệu với buffer size tối ưu
    /// </summary>
    public async Task<byte[]> ReceiveAsync(int bufferSize = 4096, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, nameof(TcpClientHelper));

        if (_stream == null)
            throw new InvalidOperationException("Not connected to the server.");

        try
        {
            byte[] buffer = new byte[bufferSize];
            int bytesRead = await _stream.ReadAsync(buffer, cancellationToken);

            if (bytesRead == 0)
                throw new IOException("The remote server closed the connection");

            byte[] data = new byte[bytesRead];
            Array.Copy(buffer, data, bytesRead);

            return data;
        }
        catch (Exception ex) when (ex is IOException || ex is SocketException)
        {
            throw new IOException($"Error receiving data from {_server}:{_port}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đóng kết nối
    /// </summary>
    public void Close()
    {
        _stream?.Close();
        _client?.Close();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _stream?.Dispose();
        _client?.Dispose();
        _disposed = true;

        GC.SuppressFinalize(this);
    }
}