using ClientLibrary.Clients;
using ClientLibrary.RPC;
using Common.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleClient.Login;
using SimpleClient.Login.ClientServices;

namespace SimpleClient.Extensions;

public static class DependencyManager
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IClientLoginService, ClientLoginService>();
        services.AddSingleton<ISerializeService, ProtobufSerializeService>();
        services.AddSingleton<RpcClient, TcpClient>( provider => new(configuration.GetSection("TcpServer")["HostAddress"]!, int.Parse(configuration.GetSection("TcpServer")["Port"]!)));
        services.AddSingleton<IRpcService, SocketService>();

        return services;
    }
}
