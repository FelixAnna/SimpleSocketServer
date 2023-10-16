namespace SimpleServer.Authorization;

public interface IAuthorizeService
{
    bool Authorize(string username, string password);
}
