using System;
using System.Collections.Generic;

namespace Hive.NET.Core.Configuration.Storage;

public interface IHiveStorageProvider
{
    void UpsertHive(Guid id, Components.Hive hive);
    Components.Hive? GetHive(Guid id);
    List<Components.Hive> GetAllHives();
}