// using System.Threading.Channels;
// using MongoDB.Bson;
//
// namespace Frogbot.Database.Models;
//
// public class Guild(string guildId, Roles roles, Channels channels)
// {
//     public ObjectId Id { get; set; }
//     public string GuildId { get; set; } = guildId;
//     public Roles Roles { get; set; } = roles;
//     public required Channels Channels { get; set; }
// }
//
// public abstract class Roles(string timeout, string announcement)
// {
//     public string Timeout { get; private set; } = timeout;
//     public string Announcement { get; private set; } = announcement;
// }
//
// public abstract class Channels(string announcement, string logs, string tempchat)
// {
//     public string Announcement { get; private set; } = announcement;
//     public string Logs { get; private set; } = logs;
//     public string Tempchat { get; private set; } = tempchat;
// }