// using System;
// using System.IO;
// using NetCord;
// using NetCord.Gateway;
// using NetCord.Logging;
// using NetCord.Rest;
// using NetCord.Services;
// using NetCord.Services.ApplicationCommands;
// using Microsoft.Extensions.Configuration;
// using System.Text.Json;
// using System.Threading;
// using System.Threading.Tasks;
// using Frogbot;
//
// namespace Frogbot.Listeners;
//
// public class Events
// {
//     private static async Task EventListener(GatewayClient client)
//     {
//         client.GuildScheduledEventCreate += async eventInfo =>
//         {
//             await client.Rest.SendMessageAsync(1401795287340810263,
//                 $"New event created: https://discord.com/events/{eventInfo.GuildId}/{eventInfo.Id}");
//         };
//     }
// }