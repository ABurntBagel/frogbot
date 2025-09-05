using System.Threading.Channels;
using MongoDB.Bson;

namespace Frogbot.Database.Models;

public class Guild(ulong guildId, Roles roles, Channels channels)
{
    public ObjectId Id { get; set; }
    // private string GuildId { get; private set ; } = guildId;
    public string GuildId
    {
        get { return guildId; }
        set
        {

        }

    }
    private Roles Roles { get; set; } = roles;
    private Channels Channels { get; set; } = channels;
}

public abstract class Roles(string timeout, string announcement)
{
    public string Timeout { get; private set; } = timeout;
    public string Announcement { get; private set; } = announcement;
}

public abstract class Channels(string announcement, string logs, string tempchat)
{
    public string Announcement { get; private set; } = announcement;
    public string Logs { get; private set; } = logs;
    public string Tempchat { get; private set; } = tempchat;
}