using MongoDB.Driver;

namespace Frogbot;

public class Configuration
{
    public MongoClient Client { get; private set; } = new MongoClient("mongodb://admin:admin@localhost:27017"); // TODO Move connection string -> .env
    public IMongoDatabase DbObject { get; private set; } = Client.GetDatabase("frogbot"); // TODO Move connection string -> .env

    public RoleConfig Roles { get; set; }
    public ChannelConfig Channels { get; set; }

    // Default values for auto configuration
    public class RoleConfig
    {
        public string SleepBonk { get; set; } = "Sleep Bonk";
        public string UpdatePing { get; set; } = "Updates";
    }

    public class ChannelConfig
    {
        public string Announcements { get; set; } = "announcements";
        public string TempChat { get; set; } = "temp-chat";
    }
}