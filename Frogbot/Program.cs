using System;
using System.IO;
using NetCord;
using NetCord.Gateway;
using NetCord.Logging;
using NetCord.Rest;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

namespace Frogbot;

public class Program
{
    public static GatewayClient Client { get; private set; } = null!;

    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        var services = builder.Services;

        services
            .AddOptions<Configuration>();

        services
            .AddApplicationCommands()
            .AddDiscordGateway(options => options.Intents = GatewayIntents.AllNonPrivileged | GatewayIntents.MessageContent)
            .AddGatewayHandlers(typeof(Program).Assembly);

        IHost host = builder.Build();

        host.AddModules(typeof(Program).Assembly);

        await host.RunAsync();
    }
}