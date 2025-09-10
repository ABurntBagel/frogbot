// using MongoDB.Bson;
// using MongoDB.Bson.Serialization;
// using MongoDB.Bson.Serialization.Attributes;
// using MongoDB.Bson.Serialization.Serializers;
// using MongoDB.Driver;
// using System.Threading.Tasks;
// using Frogbot.Database.Models;
// using Frogbot.Database.Services;
//
// internal class Program
// {
//     public static async Task Main(string[] args)
//     {
//         MongoClient client = new("mongodb://admin:admin@localhost:27017");
//         IMongoDatabase database = client.GetDatabase("frogbot");
//
//         List<User> users = UserService.CreateUsers(5);
//
//         List<string> collections = [];
//
//         var collection = "1401795286116073553";
//
// // var collectionExists = collections.AsEnumerable()
// //                                   .Contains(collection);
//
// // if (!collections.AsEnumerable()
// //                 .Contains(collection))
// // {
// //     database.CreateCollection(collection);
// // }
//
//         UserService userService = new(database, collection);
//         await userService.InsertManyAsync(users);
//     }
// }