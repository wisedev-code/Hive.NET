using System.Collections.Concurrent;

namespace Hive.NET.Core.Manager;

internal sealed class HiveManager : IHiveManager
{
    private ConcurrentDictionary<Guid, Components.Hive> HiveStorage { get; } = new();

    private static HiveManager _instance = new();

    public static HiveManager GetInstance()
    {
        _instance ??= new HiveManager();
        return _instance;
    }

    public void AddHive(Guid id, Components.Hive hive) => _instance.HiveStorage.TryAdd(id,hive);
    public Components.Hive GetHive(Guid id) => _instance.HiveStorage[id];
}