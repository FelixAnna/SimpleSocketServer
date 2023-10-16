using Common;
using Common.Serialization;
using Models.dto;
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
                var requestWithActualData = serializeService.Deserialize<RequestData<LoginReqProto>>(data);
                var response = new LoginHandler(authorizeService, serializeService).Handle(requestWithActualData.Data!);

                Console.WriteLine($"Request handled:{requestWithActualData.Data!.InSeqNum}");

                byte[] dataToSend = serializeService.Serialize(response);
                await client.SendResponseAsync(dataToSend);

                Console.WriteLine($"Response status: {response.IsOk}, {response.ErrMsg}, {response.OutSeqNum}");
            }
            else
            {
                throw new Exception("Unsupported request type.");
            }
        }
    }

    internal void Stop(CancellationToken token)
    {
        foreach (var client in clientList)
        {
            client.Disconnect(false);
            client.Dispose();
        }
    }
}
