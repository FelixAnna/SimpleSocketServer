// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Models.dto;
using SimpleClient.DependencyInjections.Extensions;
using SimpleClient.Login;

Thread.Sleep(1000); //ensure for server start

var serviceCollection = new ServiceCollection();
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

DependencyManager.RegisterDependencies(serviceCollection, configuration);

var cancellationTokenSource = new CancellationTokenSource();
var cancelationToken = cancellationTokenSource.Token;

var serviceProvider = serviceCollection.BuildServiceProvider();
using var loginService = serviceProvider.GetService<ILoginService>()!;

await loginService.LoginAsync(new LoginReqProto() { User = "admin", Password = "admin", InSeqNum = 1 }, cancelationToken);
await loginService.LoginAsync(new LoginReqProto() { User = "annoymous", Password = "annoymous", InSeqNum = 2 }, cancelationToken);

Console.ReadLine();