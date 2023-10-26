using SimpleClient.Clients;

namespace ClientLibrary.Sockets;

public sealed class TcpClient : RpcClient
{
    private readonly SemaphoreSlim _sendLock;
    private readonly IRpcService _rpcService;
    public TcpClient(string hostAddress, int port)
    {
        _rpcService = new SocketService(hostAddress, port);
        _sendLock = new SemaphoreSlim(1, 1);
    }

    public override async Task<byte[]> SendRequestAsync(byte[] dataToSend, CancellationToken cancellationToken)
    {
        await _rpcService.EnsureConnectedAsync(cancellationToken);

        try
        {
            _sendLock.Wait(cancellationToken);
            await _rpcService.SendAsync(dataToSend);
            return await _rpcService.ReceiveAsync(cancellationToken);
        }
        finally
        {
            _sendLock.Release();
        }
    }

    public override void Dispose()
    {
        _rpcService.Dispose();
    }
}
