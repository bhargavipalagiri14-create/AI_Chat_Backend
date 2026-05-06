using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Backend.Models;
public class MongoDbService
{
    public IMongoCollection<ChatSession> Chats { get; }

    public MongoDbService(IConfiguration config)
    {
        var connectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")
            ?? config["MongoDbSettings:ConnectionString"];

        var databaseName = Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME")
            ?? config["MongoDbSettings:DatabaseName"];

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Mongo connection string not found");

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new Exception("Mongo database name not found");

        if (databaseName != databaseName.ToLowerInvariant())
            throw new Exception($"Mongo database name must be lowercase to avoid casing conflicts. Use '{databaseName.ToLowerInvariant()}' instead of '{databaseName}'.");

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);

        Chats = database.GetCollection<ChatSession>("chats");
    }
}
