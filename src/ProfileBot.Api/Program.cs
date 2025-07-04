using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using ProfileBot.Application.Extensions;
using ProfileBot.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration);

//NetCord
builder.Services
    .AddDiscordGateway()
    .AddApplicationCommands();

var host = builder.Build();


//NetCord
host.AddModules(typeof(Program).Assembly)
    .UseGatewayEventHandlers();


await host.RunAsync();
