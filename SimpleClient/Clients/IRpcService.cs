namespace SimpleClient.Clients;

public interface IRpcService : IDisposable
{
    Task EnsureConnectedAsync(CancellationToken cancellationToken);
    Task<byte[]> ReceiveAsync(CancellationToken cancellationToken);
    Task SendAsync(byte[] dataToSend);
}