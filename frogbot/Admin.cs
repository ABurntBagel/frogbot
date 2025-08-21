using System.Text.RegularExpressions;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Frogbot;

public class Admin : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("ping", "Ping!")]
    public static string Ping() => "Pong! 🏓";

    [SlashCommand("c", "c")]
    public static string C(
        [SlashCommandParameter(Description = "Duration (format such as 30m, 2h, 1d, etc.")]
        string duration)
    {
        try
        {
            var dur = CalculateResolveTime(duration);

            return $"{dur}";
        }
        catch (ArgumentException ex)
        {
            return ex.Message;
        }
    }

    [SlashCommand("bonk", "Hit 'em with the sleep bonk.", DefaultGuildUserPermissions = Permissions.ModerateUsers,
        Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> Bonk(
        [SlashCommandParameter(Description = "The user to bonk")]
        User user,
        [SlashCommandParameter(Description = "Duration (format such as 30m, 2h, 1d, etc.")]
        string duration)
    {
        try
        {
            const ulong bonkRole = 1401795286116073558; //Temporary for dev purposes
            Console.WriteLine($"Got: {user.Id}");
            var guild = Context.Guild!;
            var guildUser = await guild.GetUserAsync(user.Id);
            var resolveTime = CalculateResolveTime(duration);
            var embed = new EmbedProperties()
                .WithTimestamp(DateTimeOffset.UtcNow)
                .WithFooter(new EmbedFooterProperties()
                    .WithText(""))
                .WithImage("https://pbs.twimg.com/media/EaYf1a_UcAEpOwC.jpg");
            var message = new InteractionMessageProperties();

            if (guildUser.RoleIds.Any(role => role == bonkRole))
            {
                embed.WithDescription($"<@{guildUser.Id}> is already sleep bonked.");
                return
                    message.AddEmbeds(embed);
            }

            await guildUser.AddRoleAsync(bonkRole);
            await RoleStorage.Add(guild.Id, guildUser.Id, bonkRole, DateTime.Now, resolveTime);

            embed
                .WithDescription($"<@{guildUser.Id}> has been sleep bonked.")
                .WithColor(new(0x5865F2))
                .WithAuthor(new EmbedAuthorProperties()
                    .WithName(guildUser.Username)
                    .WithIconUrl(guildUser.DefaultAvatarUrl.ToString())
                )
                .AddFields(
                    new EmbedFieldProperties()
                        .WithName("Expires")
                        .WithValue(resolveTime.ToString())
                        .WithInline(),
                    new EmbedFieldProperties()
                        .WithName("Expires")
                        .WithValue(duration)
                        .WithInline());

            return message.AddEmbeds(embed);

            // return
            //     $"Got user: {guildUser.Username}\nCreatedTime: {DateTime.Now}\nResolveTime: {resolveTime}\nRole(s) Added: {bonkRole}";
        }
        catch (Exception ex)
        {
            return new InteractionMessageProperties().AddEmbeds(new EmbedProperties().WithDescription(ex.Message));
        }
    }

    private static DateTime CalculateResolveTime(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentException("Duration cannot be empty.");

        var regex = new Regex(@"^(\d+)(s|m|h|d)$", RegexOptions.Compiled);

        var match = regex.Match(input);
        if (!match.Success)
            throw new ArgumentException("Invalid format.");

        if (!int.TryParse(match.Groups[1].Value, out var parsedValue) && parsedValue < 0)
            throw new ArgumentException("Invalid time value.");


        var unit = match.Groups[2].Value;
        var duration = parsedValue * GetMultiplier(unit);
        var resolveTime = DateTime.Now.AddSeconds(duration);

        return resolveTime;

        // Debug return
        // return $"Got val: {parsedValue}\nunit: {unit}\n multiplier: {GetMultiplier(unit)}\n duration: {parsedValue * GetMultiplier(unit)}\n resolveTime: {resolveTime}";
    }

    private static int GetMultiplier(string unit) => unit.ToLower() switch
    {
        "s" => 1,
        "m" => 60,
        "h" => 3600,
        "d" => 86400,
        _ => throw new ArgumentException($"Invalid unit: {unit}"),
    };
}