using System.Net.Sockets;
using System.Text;

namespace Common;

public static class SocketExtension
{
    public static async Task<byte[]> GetSocketResponseAsync(this Socket socket, CancellationToken cancellationToken)
    {
        byte[] responseBytes = new byte[256];

        var results = new List<byte>();

        while (true)
        {
            int bytesReceived = await socket.ReceiveAsync(responseBytes, SocketFlags.None, cancellationToken);

            if (bytesReceived == 0) break;

            results.AddRange(responseBytes[0..bytesReceived]);
            if(bytesReceived < responseBytes.Length)
            {
                break;
            }
        }

        return results.ToArray();
    }

    public static async Task SendResponseAsync(this Socket socket, byte[] dataToSend)
    {
        int bytesSent = 0;
        while (bytesSent < dataToSend.Length)
        {
            bytesSent += await socket.SendAsync(dataToSend.AsMemory(bytesSent), SocketFlags.None);
        }
    }
}
