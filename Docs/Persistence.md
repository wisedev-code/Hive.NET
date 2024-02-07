# Persistence Package Overview

The Persistence package extends the functionality of the Hive.NET ackage by providing storage solutions for maintaining the state of hives. This ensures that even in scenarios such as application failures or redeployments, the state of hive entities, including their collections of work items, is preserved.

## PersistenceBootstrapper

The `PersistenceBootstrapper` class offers extension methods for conveniently configuring different storage providers for hive state persistence.

### Available Methods

- **IncludeHiveMongoStorage**: Configures MongoDB storage for hive state persistence.
- **IncludeHiveSqlStorage**: Configures SQL storage for hive state persistence.
- **IncludeHiveFileStorage**: Configures file storage for hive state persistence.

#### Example Usage

```csharp
// Include MongoDB storage for hive state persistence
services.IncludeHiveMongoStorage(connectionString, databaseName, collectionName);

// Include SQL storage for hive state persistence
services.IncludeHiveSqlStorage(connectionString);

// Include file storage for hive state persistence
services.IncludeHiveFileStorage(filePath);
```

## Available Storage Providers

### MongoHiveStorageProvider

The `MongoHiveStorageProvider` utilizes MongoDB as a storage backend for hive state persistence. It offers the following features:

- **UpsertHive**: Stores or updates the state of a hive with the specified ID.
- **GetHive**: Retrieves the state of a hive by its ID.
- **GetAllHives**: Retrieves the state of all hives stored in the MongoDB collection.

### SqlHiveStorageProvider

The `SqlHiveStorageProvider` leverages SQL databases for storing hive state. Key features include:

- **UpsertHive**: Inserts or updates the state of a hive in the SQL database.
- **GetHive**: Retrieves the state of a hive by its ID.
- **GetAllHives**: Retrieves the state of all hives stored in the SQL database.

### FileHiveStorageProvider

The `FileHiveStorageProvider` stores hive state in a file on the file system. It provides the following capabilities:

- **UpsertHive**: Adds or updates the state of a hive in the file.
- **GetHive**: Retrieves the state of a hive by its ID.
- **GetAllHives**: Retrieves the state of all hives stored in the file.