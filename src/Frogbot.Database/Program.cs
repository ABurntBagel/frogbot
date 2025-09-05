using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Threading.Tasks;
using Frogbot.Database.Models;
using Frogbot.Database.Services;

MongoClient client = new("mongodb://admin:admin@localhost:27017");
IMongoDatabase database = client.GetDatabase("frogbot");

List<User> users = UserService.CreateUsers(5);

UserService userService = new(database, "users");
await userService.InsertUsersAsync(users);