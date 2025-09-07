using Frogbot.Database.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Frogbot.Database.Services;


public class UserService(IMongoDatabase database, string collectionName) : BaseService<User>(database, collectionName)
{
    public static List<User> CreateUsers(int numUsers)
    {
        List<User> users = [];

        for (var i = 0; i < numUsers; i++)
        {
            Random random = new();

            User user = new(RandId(), "New Data!", RandId());

            // Console.WriteLine(user);

            users.Add(user);
            continue;

            string RandId() => random.NextInt64()
                                     .ToString();
        }
        
        Console.WriteLine($"Added {users.Count} items to list.");
        return users;
    }
}