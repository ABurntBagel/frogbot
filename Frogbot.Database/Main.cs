using System.Diagnostics.CodeAnalysis;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Threading.Tasks;
using Frogbot.Database.Models;
using Frogbot.Database.Services;

namespace Frogbot.Database;

public static class Db
{
    public static MongoClient Client { get; private set; } = new MongoClient("mongodb://admin:admin@localhost:27017");
    public static IMongoDatabase DbObject { get; private set; } = Client.GetDatabase("frogbot");

    public static void Main(string[] args) => Console.WriteLine("DB SERVICE ASSIGNED CLIENT");
}