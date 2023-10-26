using ClientLibrary.Clients;
using ClientLibrary.RPC;
using Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serialization;
using Serialization.Protobuf;
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
        services.AddSingleton<RpcClient, TcpClient>( provider => new(IPUtils.GetIpAddress(configuration.GetSection("TcpServer")["HostAddress"]), int.Parse(configuration.GetSection("TcpServer")["Port"]!)));
        services.AddSingleton<IRpcService, SocketService>();

        return services;
    }
}
