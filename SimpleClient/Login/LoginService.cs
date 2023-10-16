using Models.dto;
using SimpleClient.Login.ClientServices;

namespace SimpleClient.Login;

public class LoginService : ILoginService
{
    private readonly IClientLoginService clientLoginService;

    public LoginService(IClientLoginService clientLoginService)
    {
        this.clientLoginService = clientLoginService;
    }

    public async Task<LoginRespProto?> LoginAsync(LoginReqProto request, CancellationToken? cancellationToken = null)
    {
        /* *
         * 1. validate request
         * 2. send data to socket service: establish connection -> send message to server -> server valid user+password -> parse response
         * 3. convert response to LoginRespProto
         * 4. return
         * */
        if (!Validate(request))
        {
            return new LoginRespProto() { IsOk = false, ErrMsg = $"Invalid parameter: {nameof(request)}" };
        }

        return await clientLoginService.LoginAsync(request, cancellationToken ?? CancellationToken.None);
    }

    private static bool Validate(LoginReqProto request)
    {
        return request != null && !string.IsNullOrEmpty(request.User) && !string.IsNullOrEmpty(request.Password);
    }

    public void Dispose() => clientLoginService.Dispose();
}
