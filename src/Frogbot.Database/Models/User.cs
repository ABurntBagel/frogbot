using MongoDB.Bson;

namespace Frogbot.Database.Models;

public class User(string userId, string username, string guildId)
{
    public ObjectId Id { get; set; }
    private string UserId { get; set; } = userId;
    private string Username { get; set; } = username;
    private string GuildId { get; set; } = guildId;

    public override string ToString()
    {
        return $"== Got User Object ==\nObjectId:\t{this.UserId}\nUsername:\t{this.Username}\nGuildId:\t{this.GuildId}";
    }
}