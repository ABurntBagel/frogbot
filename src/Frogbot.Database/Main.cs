using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Threading.Tasks;
using Frogbot.Database.Models;
using Frogbot.Database.Services;

namespace Frogbot.Database;

public class Main
{
    public MongoClient Client = new("mongodb://admin:admin@localhost:27017");
    public IMongoDatabase Database = Client.GetDatabase("frogbot");
    public static async Task DB()
    {


        List<User> users = UserService.CreateUsers(5);

        List<string> collections = [];

        var collection = "1401795286116073553";

        UserService userService = new(Database, collection);
        await userService.InsertManyAsync(users);
    }
}