// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleServer;
using SimpleServer.Extensions;

var serviceCollection = new ServiceCollection();
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

DependencyManager.RegisterDependencies(serviceCollection, configuration);

Console.WriteLine("Starting server:");
var serviceProvider = serviceCollection.BuildServiceProvider();
var simpleTcpServer = serviceProvider.GetService<SimpleTcpServer>()!;
simpleTcpServer.Logger = Console.WriteLine;
simpleTcpServer.clientRequestHandler = serviceProvider.GetService<ClientRequestHandler>()!;
simpleTcpServer.Start();

Console.WriteLine("Server started, Press [Escape] to stop");

do
{
    var key = Console.ReadKey();
    if (key.Key == ConsoleKey.Escape)
    {
        simpleTcpServer.Stop();
        break;
    }
} while (true);

Console.Read();