using MongoDB.Bson;

namespace Frogbot.Database.Models;

public class User(string userId, string username, string guildId)
{
    public ObjectId Id { get; set; }
    public string UserId { get; set; } = userId;
    public string Username { get; set; } = username;
    public string GuildId { get; set; } = guildId;

    public override string ToString()
    {
        return $"== New User Object ==\n" +
               $"UserId:\t{this.UserId}\n" +
               $"Username:\t{this.Username}\n" +
               $"GuildId:\t{this.GuildId}";
    }
}