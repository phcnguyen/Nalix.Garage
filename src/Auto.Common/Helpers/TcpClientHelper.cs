using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Auto.Common.Helpers;

public class TcpClientHelper(string server, int port)
{
    private readonly string _server = server;
    private readonly int _port = port;
    private TcpClient _client;
    private NetworkStream _stream;

    public async Task ConnectAsync()
    {
        _client = new TcpClient();
        await _client.ConnectAsync(_server, _port);
        _stream = _client.GetStream();
    }

    public async Task SendAsync(byte[] data)
    {
        if (_stream == null)
            throw new InvalidOperationException("Not connected to the server.");

        await _stream.WriteAsync(data);
    }

    public async Task<byte[]> ReceiveAsync()
    {
        if (_stream == null)
            throw new InvalidOperationException("Not connected to the server.");

        byte[] buffer = new byte[256];
        int bytesRead = await _stream.ReadAsync(buffer);

        byte[] data = new byte[bytesRead];
        Array.Copy(buffer, data, bytesRead);

        return data;
    }

    public void Close()
    {
        _stream?.Close();
        _client?.Close();
    }
}