using Hive.NET.Core.Configuration.Storage;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Hive.NET.Persistence;

public class MongoHiveStorageProvider : IHiveStorageProvider
{
    private readonly IMongoCollection<HiveEntity> _collection;

    public MongoHiveStorageProvider(string connectionString, string databaseName, string collectionName)
    {
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<HiveEntity>(collectionName);

        EnsureIndexes();
    }

    public void UpsertHive(Guid id, Core.Components.Hive? hive)
    {
        var filter = Builders<HiveEntity>.Filter.Eq(entity => entity.Id, id);
        var update = Builders<HiveEntity>.Update.Set(entity => entity.Hive, hive).SetOnInsert(entity => entity.Id, id);
        _collection.UpdateOne(filter, update, new UpdateOptions { IsUpsert = true });
    }

    public Core.Components.Hive GetHive(Guid id)
    {
        var filter = Builders<HiveEntity>.Filter.Eq(entity => entity.Id, id);
        var result = _collection.Find(filter).FirstOrDefault();
        return result?.Hive!;
    }

    public List<Core.Components.Hive> GetAllHives()
    {
        var hives = _collection.Find(new BsonDocument()).ToList();
        return hives.ConvertAll(entity => entity.Hive);
    }

    // Other methods as needed

    private void EnsureIndexes()
    {
        var indexKeysDefinition = Builders<HiveEntity>.IndexKeys.Ascending(entity => entity.Id);
        var indexModel = new CreateIndexModel<HiveEntity>(indexKeysDefinition);
        _collection.Indexes.CreateOne(indexModel);
    }
}

public class HiveEntity
{
    public Guid Id { get; set; }
    public Core.Components.Hive Hive { get; set; }
}
