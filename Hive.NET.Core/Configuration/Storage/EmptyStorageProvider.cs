using System;
using System.Collections.Generic;

namespace Hive.NET.Core.Configuration;

public class EmptyStorageProvider : IHiveStorageProvider
{
    public void AddHive(Guid id, Components.Hive hive)
    {
        //nothing
    }

    public List<Components.Hive> GetHives()
    {
        return new List<Components.Hive>();
    }
}