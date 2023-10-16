using Common;
using System.Net.Sockets;

namespace ClientLibrary.RPC;

public sealed class SocketService : IRpcService
{
    private readonly string hostAddress;
    private readonly int port;

    private readonly Socket _socket;

    public SocketService(string hostAddress, int port)
    {
        this.port = port;
        this.hostAddress = hostAddress;

        _socket = new(SocketType.Stream, ProtocolType.Tcp);
    }

    public async Task<byte[]> ReceiveAsync(CancellationToken cancellationToken)
    {   
        return await _socket.GetSocketResponseAsync(cancellationToken);
    }

    public async Task SendAsync(byte[] dataToSend)
    {
        await _socket.SendResponseAsync(dataToSend);
    }

    public async Task EnsureConnectedAsync(CancellationToken cancellationToken)
    {
        if (!_socket.Connected)
        {
            await _socket.ConnectAsync(hostAddress, port, cancellationToken);
        }
    }

    public void Dispose()
    {
        if (_socket.Connected)
        {
            _socket.Disconnect(false);
            _socket.Close();
        }

        _socket.Dispose();
    }
}
