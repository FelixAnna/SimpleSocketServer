namespace SimpleClient.Clients;

public abstract class RpcClient : IDisposable
{
    public abstract Task<byte[]> SendRequestAsync(byte[] dataToSend, CancellationToken cancellationToken);
    public abstract void Dispose();
}
