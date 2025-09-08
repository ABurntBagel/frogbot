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

namespace Frogbot;

public class Program
{
    public static GatewayClient Client { get; private set; } = null!;
    // public static ConsoleLogger Logger { get; private set; } = null!;

    public static async Task Main(string[] args)
    {
        // Check if appsettings.json exists and if not, create one
        if (!File.Exists("appsettings.json"))
        {
            Console.WriteLine("No appsettings.json found, creating...");
            Console.WriteLine(
                "Please enter your Discord bot token. Visit https://discord.com/developers/applications if you need one.");
            Console.Write("Bot Token > ");

            var token = Console.ReadLine()
                ?.Trim() ?? "";

            var configData = new
            {
                Discord = new
                {
                    Token = token
                }
            };

            Console.WriteLine("Configuration created. You can find it in the project root.");

            await File.WriteAllTextAsync("appsettings.json", JsonSerializer.Serialize(configData,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }

        var botToken = GetBotToken();
        if (string.IsNullOrWhiteSpace(botToken)) throw new Exception("Bot token not found.");
        Console.WriteLine("Configuration loaded.");


        // Create the client
        Client = new GatewayClient(new BotToken(botToken), new GatewayClientConfiguration
        {
            Intents = GatewayIntents.AllNonPrivileged,
            Logger = new ConsoleLogger(),
        });

        // Create the application command service and load modules
        ApplicationCommandService<ApplicationCommandContext> applicationCommandService = new();
        applicationCommandService.AddModules(typeof(Program).Assembly);

        // Add the handler to handle interactions
        Client.InteractionCreate += async interaction =>
        {
            // Check if the interaction is an application command interaction
            if (interaction is not ApplicationCommandInteraction applicationCommandInteraction)
                return;

            // Execute the command
            var result =
                await applicationCommandService.ExecuteAsync(new ApplicationCommandContext(
                    applicationCommandInteraction,
                    Client));

            // Check if the execution failed
            if (result is not IFailResult failResult)
                return;

            // Return the error message to the user if the execution failed
            try
            {
                await interaction.SendResponseAsync(InteractionCallback.Message(failResult.Message));
            }
            catch
            {
                // ignored
            }
        };

        // Monitor for new guild events
        // Need to rework this later, event handling should be broken out into modules
        Client.GuildScheduledEventCreate += async eventInfo =>
        {
            await Client.Rest.SendMessageAsync(1401795287340810263,
                $"New event created: https://discord.com/events/{eventInfo.GuildId}/{eventInfo.Id}");
        };

        await applicationCommandService.RegisterCommandsAsync(Client.Rest, Client.Id);

        var worker = new RoleWorker();
        _ = worker.StartAsync(CancellationToken.None);

        await Client.StartAsync();
        await Task.Delay(-1);
        return;

        string? GetBotToken()
        {
            if (File.Exists("/.dockerenv") || File.Exists(".env"))
                return Environment.GetEnvironmentVariable("DISCORD_TOKEN");

            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Load bot token from configuration and validate
            return config["Discord:Token"];
        }
    }
}