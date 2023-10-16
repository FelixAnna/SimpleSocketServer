namespace SimpleServer.Authorization;

public class SimpleAuthorizeService : IAuthorizeService
{
    public bool Authorize(string username, string password)
    {
        return username == "admin" && password == "admin";
    }
}
