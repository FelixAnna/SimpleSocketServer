using Models.dto;
using Serialization;
using SimpleClient.Clients;
using SimpleClient.Models;

namespace SimpleClient.Login.ClientServices;

public class ClientLoginService : IClientLoginService
{
    private readonly RpcClient _client;
    private readonly ISerializeService _serializeService;

    public ClientLoginService(RpcClient client, ISerializeService serializeService)
    {
        _client = client;
        _serializeService = serializeService;
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task<LoginRespProto> LoginAsync(LoginReqProto request, CancellationToken cancellationToken)
    {
        var requestData = new RequestData<LoginReqProto>() { Data = request, Type = ERequestType.Login };
        var result = await _client.SendRequestAsync(_serializeService.Serialize(requestData), cancellationToken);
        var loginResult = _serializeService.Deserialize<LoginRespProto>(result);
        Console.WriteLine($"Login status: {loginResult.IsOk}, {loginResult.ErrMsg}, {loginResult.OutSeqNum}");
        return loginResult;
    }
}
