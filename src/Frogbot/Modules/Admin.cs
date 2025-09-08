using System.Text.RegularExpressions;
using Frogbot.Database;
using Frogbot.Database.Services;
using MongoDB.Driver;
using NetCord;
using NetCord.Gateway;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

namespace Frogbot.Modules;

public class Admin : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("ping", "Ping!")]
    public static string Ping() => "Pong! 🏓";

    private const ulong BonkRole = 1401795286116073558; //TODO Remove and pull from db


    #region SleepBonkCommand

    #region /bonk

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
            Console.WriteLine($"Got: {user.Id}");
            Guild guild = this.Context.Guild!;
            GuildUser guildUser = await guild.GetUserAsync(user.Id);
            DateTime resolveTime = CalculateResolveTime(duration);
            EmbedProperties embed = new EmbedProperties()
                                    .WithTimestamp(DateTimeOffset.UtcNow)
                                    .WithFooter(new EmbedFooterProperties().WithText(""))
                                    .WithImage("https://pbs.twimg.com/media/EaYf1a_UcAEpOwC.jpg");

            var message = new InteractionMessageProperties();

            if (guildUser.RoleIds.Any(role => role == BonkRole))
            {
                embed.WithDescription($"<@{guildUser.Id}> is already sleep bonked.");
                return
                    message.AddEmbeds(embed);
            }

            await guildUser.AddRoleAsync(BonkRole);
            await RoleStorage.Add(guild.Id, guildUser.Id, BonkRole, DateTime.Now, resolveTime);

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
                        .WithInline());

            return message.AddEmbeds(embed);
        }
        catch (Exception ex)
        {
            return new InteractionMessageProperties().AddEmbeds(new EmbedProperties().WithDescription(ex.Message));
        }
    }

    #endregion

    #region /unbonk

    [SlashCommand("unbonk", "Unbonk a user.", DefaultGuildUserPermissions = Permissions.ModerateUsers,
        Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> Unbonk(
        [SlashCommandParameter(Description = "The user to un-bonk")]
        User user)
    {
        GuildUser guildUser = await this.Context.Guild!.GetUserAsync(user.Id);
        var result = guildUser.RoleIds.Any(role => role == BonkRole);

        if (result)
        {
            await guildUser.RemoveRoleAsync(BonkRole);
            return new InteractionMessageProperties().AddEmbeds(
                new EmbedProperties().WithDescription($"<@{guildUser.Id}> is no longer sleep bonked."));
        }
        else
        {
            return new InteractionMessageProperties().AddEmbeds(
                new EmbedProperties().WithDescription($"<@{guildUser.Id}> is not sleep bonked!"));
        }
    }

    #endregion

    #endregion

    [SlashCommand("dump", "Dump user data to db", Contexts = [InteractionContextType.Guild])]
    public async Task<InteractionMessageProperties> Dump([SlashCommandParameter(Description = "target user")] User user)
    {
        try
        {
            IMongoDatabase database = Db.DbObject;

            var userService = new UserService(database, "users");

            Guild guild = this.Context.Guild!;
            var targetUser = new Database.Models.User(user.Id.ToString(), user.Username, guild.Id.ToString());

            await userService.InsertAsync(targetUser);

            return new InteractionMessageProperties().AddEmbeds(
                new EmbedProperties()
                    .WithDescription(
                        $"UserId:\t{user.Id.ToString()}\n Username:\t{user.Username}\n GuildId:\t{guild.Id.ToString()}\nIsBot:\t{user.IsBot}")
                    .WithFooter(new EmbedFooterProperties().WithText($"_id:{targetUser.Id}")));
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
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