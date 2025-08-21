using System.Text.RegularExpressions;
using NetCord;
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
    public async Task<string> Bonk(
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

            if (guildUser.RoleIds.Any(role => role == bonkRole))
            {
                return "User is already sleeping."; // Next to insert a record to data JSON.
            }

            await guildUser.AddRoleAsync(bonkRole);

            return
                $"Got user: {guildUser.Username}\nCreatedTime: {DateTime.Now}\nResolveTime: {resolveTime}\nRole(s) Added: {bonkRole}";
        }
        catch (Exception ex)
        {
            return $"{ex}";
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