using System;
using System.Collections.Generic;

namespace Hive.NET.Core.Configuration.Storage;

public class EmptyStorageProvider : IHiveStorageProvider
{
    public void UpsertHive(Guid id, Components.Hive? hive)
    {
        //nothing
    }

    public Components.Hive? GetHive(Guid id)
    {
        return null!;
    }

    public List<Components.Hive?> GetAllHives()
    {
        return null!;
    }
}