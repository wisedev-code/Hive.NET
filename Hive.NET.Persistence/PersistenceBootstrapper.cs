using Hive.NET.Core.Configuration.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.NET.Persistence;

public static class PersistenceBootstrapper
{
    public static IServiceCollection IncludeHiveFileStorage(
        this IServiceCollection collection,
        string filePath)
    {
        collection.AddSingleton<IHiveStorageProvider>(new FileHiveStorageProvider(filePath));
        return collection;
    }
    
    public static IServiceCollection IncludeHiveMongoStorage(
        this IServiceCollection collection,
        string connectionString,
        string databaseName,
        string collectionName)
    {
        collection.AddSingleton<IHiveStorageProvider>(new MongoHiveStorageProvider(connectionString, databaseName, collectionName));
        return collection;
    }
    
    public static IServiceCollection IncludeHiveSqlStorage(
        this IServiceCollection collection,
        string connectionString)
    {
        collection.AddSingleton<IHiveStorageProvider>(new SqlHiveStorageProvider(connectionString));
        return collection;
    }
}