using System;
using System.Collections.Generic;

namespace Hive.NET.Core.Configuration;

public interface IHiveStorageProvider
{
    void AddHive(Guid id, Components.Hive hive);
    List<Components.Hive> GetHives();
}