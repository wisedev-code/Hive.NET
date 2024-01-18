using System;

namespace Hive.NET.Core.Manager;

public interface IHiveManager
{
    public void AddHive(Guid id, Components.Hive hive);
    public Components.Hive GetHive(Guid id);
}