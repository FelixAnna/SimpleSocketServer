using Common.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleServer.Authorization;
using SimpleServer.Handlers;

namespace SimpleServer.Extensions;

public static class DependencyManager
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ClientRequestHandler>();
        services.AddSingleton<SimpleTcpServer>(provider => new(configuration.GetSection("TcpServer")["HostAddress"]!, int.Parse(configuration.GetSection("TcpServer")["Port"]!)));
        services.AddSingleton<LoginHandler>();
        services.AddSingleton<IAuthorizeService, SimpleAuthorizeService>();
        services.AddSingleton<ISerializeService, ProtobufSerializeService>();

        return services;
    }
}
