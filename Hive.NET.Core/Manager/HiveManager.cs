using System.Collections.Concurrent;

namespace Hive.NET.Core.Manager;

public class HiveManager : IHiveManager
{
    private ConcurrentDictionary<Guid, Components.Hive> HiveStorage { get; set; } = new();

    public void AddHive(Guid id, Components.Hive hive) => HiveStorage.TryAdd(id,hive);
    public Components.Hive GetHive(Guid id) => HiveStorage[id];
}