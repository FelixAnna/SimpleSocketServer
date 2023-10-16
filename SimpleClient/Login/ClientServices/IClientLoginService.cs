using Models.dto;

namespace SimpleClient.Login.ClientServices;

public interface IClientLoginService : IDisposable
{
    Task<LoginRespProto> LoginAsync(LoginReqProto request, CancellationToken cancellationToken);
}