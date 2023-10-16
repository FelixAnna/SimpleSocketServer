using Models.dto;

namespace SimpleClient.Login;

public interface ILoginService : IDisposable
{
    Task<LoginRespProto?> LoginAsync(LoginReqProto req, CancellationToken? cancellationToken = null);
}
