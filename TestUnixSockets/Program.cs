using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TestUnixSockets
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var url = MongoUrl.Create("mongodb://%2Ftmp%2Fmongodb-27017.sock/admin");
            
            var settings = MongoClientSettings.FromUrl(url);
            var client = new MongoClient(settings.WithUnixDomainSockets());
            var database = client.GetDatabase("local");
            
            Console.WriteLine("Connected");
            
            var collection = database.GetCollection<BsonDocument>("startup_log");
            var documents = await collection.Find(Builders<BsonDocument>.Filter.Empty).ToListAsync();
            
            Console.WriteLine("Fetched {0} documents", documents.Count);
        }
    }
}