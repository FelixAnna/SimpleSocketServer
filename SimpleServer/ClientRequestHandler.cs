using Common;
using Models.dto;
using Serialization;
using SimpleClient.Models;
using SimpleServer.Authorization;
using SimpleServer.Handlers;
using System.Net.Sockets;

namespace SimpleServer;

public class ClientRequestHandler
{
    private readonly IEnumerable<Socket> clientList;
    private readonly ISerializeService serializeService;
    private readonly IAuthorizeService authorizeService;

    public ClientRequestHandler(IAuthorizeService authorizeService, ISerializeService serializeService)
    {
        this.clientList = new List<Socket>();
        this.authorizeService = authorizeService;
        this.serializeService = serializeService;
    }

    public async Task HandleAsync(Socket client, CancellationToken token)
    {
        clientList.Append(client);

        while (true)
        {
            // Receive message.
            var data = await client.GetSocketResponseAsync(token);
            if (data.Length == 0)
            {
                Console.WriteLine($"Request closed:{client.RemoteEndPoint}");
                await client.DisconnectAsync(true);
                break;
            }

            var request = serializeService.Deserialize<RequestData<ReqProtoBase>>(data);

            if(request.Type == ERequestType.Login)
            {
                await HandleLoginAsync(client, data);
            }
            else
            {
                throw new Exception("Unsupported request type.");
            }
        }
    }

    private async Task HandleLoginAsync(Socket client, byte[] data)
    {
        var requestWithActualData = serializeService.Deserialize<RequestData<LoginReqProto>>(data);
        var response = new LoginHandler(authorizeService, serializeService).Handle(requestWithActualData.Data!);

        Console.WriteLine($"Request handled:{requestWithActualData.Data!.InSeqNum}");

        byte[] dataToSend = serializeService.Serialize(response);
        await client.SendResponseAsync(dataToSend);

        Console.WriteLine($"Response status: {response.IsOk}, {response.ErrMsg}, {response.OutSeqNum}");
    }

    internal void Stop()
    {
        foreach (var client in clientList)
        {
            client.Disconnect(false);
            client.Dispose();
        }
    }
}
