using System.Text.Json;
using Microsoft.Extensions.Hosting;
using NetCord.Rest;

namespace Frogbot;

public record UserRole(ulong GuildId, ulong UserId, ulong RoleId, DateTime AddedTime, DateTime ResolveTime);

public static class RoleStorage
{
    private const string FILE = "data.json";

    public static async Task Add(ulong guildId, ulong userId, ulong roleId, DateTime addedTime, DateTime resolveTime)
    {
        var roles = await GetAll();
        roles.Add(new(guildId, userId, roleId, addedTime, resolveTime));
        await File.WriteAllTextAsync(FILE, JsonSerializer.Serialize(roles));
    }

    private static async Task<List<UserRole>> GetAll()
    {
        if (!File.Exists(FILE)) return [];
        var json = await File.ReadAllTextAsync(FILE);
        return JsonSerializer.Deserialize<List<UserRole>>(json) ?? [];
    }

    public static async Task<List<UserRole>> GetDue()
    {
        var roles = await GetAll();
        return roles.Where(r => r.ResolveTime <= DateTime.Now).ToList();
    }

    public static async Task Remove(List<UserRole> toRemove)
    {
        var roles = await GetAll();
        var remaining = roles.Except(toRemove).ToList();
        await File.WriteAllTextAsync(FILE, JsonSerializer.Serialize(remaining));
    }

}
public class RoleWorker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancelToken)
    {
        Console.WriteLine($"{DateTime.Now.TimeOfDay} \t RoleWorker started.");

        while (!cancelToken.IsCancellationRequested)
        {
            var due = await RoleStorage.GetDue();
            if (due.Count != 0)
            {
                Console.WriteLine($"Found {due.Count} expired roles to process.");

                foreach (var role in due)
                {
                    // Console.WriteLine($"PROCESSING - GUILD: {role.GuildId}\nUSER: {role.UserId}\nROLE: {role.RoleId}\nADDED: {role.AddedTime}\nRESOLVE: {role.ResolveTime}");
                    await Program.Client.Rest.RemoveGuildUserRoleAsync(role.GuildId, role.UserId, role.RoleId, cancellationToken: cancelToken);

                    var user = await Program.Client.Rest.GetUserAsync(role.UserId, cancellationToken: cancelToken);

                    var embed = new EmbedProperties()
                        .WithDescription($"**Sleep bonk removed from <@{role.UserId}>**")
                        .WithColor(new(0x5865F2))
                        .WithTimestamp(DateTimeOffset.UtcNow)
                        .WithFooter(new EmbedFooterProperties()
                            .WithText(""))
                        .WithAuthor(new EmbedAuthorProperties()
                            .WithName(user.Username)
                            .WithIconUrl(user.GetAvatarUrl()?.ToString())
                        )
                        .AddFields(
                            new EmbedFieldProperties()
                                .WithName("Due At")
                                .WithValue(role.ResolveTime.ToString())
                                .WithInline(),
                            new EmbedFieldProperties()
                                .WithName("Processed At")
                                .WithValue(DateTime.Now.ToString())
                                .WithInline());

                    var message = new MessageProperties()
                        .AddEmbeds(embed);

                    await Program.Client.Rest.SendMessageAsync(1401795287340810263, message, cancellationToken: cancelToken);
                }
                await RoleStorage.Remove(due);
            }
            await Task.Delay(TimeSpan.FromSeconds(2), cancelToken);
        }
    }
}