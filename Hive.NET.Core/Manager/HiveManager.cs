using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Hive.NET.Core.Api;

[assembly: InternalsVisibleTo("Hive.NET.Tests")]
namespace Hive.NET.Core.Manager;

internal sealed class HiveManager : IHiveManager
{
    private ConcurrentDictionary<Guid, Components.Hive?> HiveStorage { get; } = new();

    private static HiveManager _instance = new();
    
    public static IHiveManager GetInstance()
    {
        return _instance;
    }

    public void AddHive(Guid id, Components.Hive? hive)
    {
        _instance.HiveStorage.TryAdd(id, hive);
    }

    public Components.Hive? GetHive(Guid id) => _instance.HiveStorage[id];

    public Components.Hive? GetHive(string name) => _instance.HiveStorage.Values.FirstOrDefault(x => x!._name == name);
    
    // ***Internals
    List<HiveDto> IHiveManager.GetHives()
    {
        return HiveStorage.Values.Select(hive => hive.MapToDto()).ToList();
    }
    
    HiveDetailsDto IHiveManager.GetHiveDetails(Guid id)
    {
        return HiveStorage[id].MapToDetailsDto();
    }

    List<BeeErrorDto> IHiveManager.GetHiveRegisteredErrors(Guid id)
    {
        return HiveStorage[id].MapToErrorsDto();
    }
}