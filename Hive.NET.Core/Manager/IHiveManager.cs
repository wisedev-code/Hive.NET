using System;
using System.Collections.Generic;
using Hive.NET.Core.Api;

namespace Hive.NET.Core.Manager;

public interface IHiveManager
{
    public void AddHive(Guid id, Components.Hive hive);
    public Components.Hive GetHive(Guid id);
    public List<HiveDto> GetHives();
}