using Models.dto;
using Serialization;
using SimpleServer.Authorization;

namespace SimpleServer.Handlers;

public class LoginHandler : AbstractHandler<LoginReqProto, LoginRespProto, IAuthorizeService>
{
    public LoginHandler(IAuthorizeService authorizeService,  ISerializeService serializeService) : base(authorizeService, serializeService)
    {
    }

    public override LoginRespProto Handle(LoginReqProto request)
    {
        bool isAuthenticated = false;
        var errorMessage = string.Empty;
        try
        {
            isAuthenticated = service.Authorize(request.User, request.Password);
        }catch (Exception ex)
        {
            errorMessage = ex.Message;
        }

        return new LoginRespProto() { IsOk = isAuthenticated, ErrMsg=errorMessage, OutSeqNum=request.InSeqNum};
    }
}
