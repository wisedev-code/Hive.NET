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
    private ConcurrentDictionary<Guid, Components.Hive> HiveStorage { get; } = new();

    private static HiveManager _instance = new();
    
    internal bool Persistence { get; init; }

    HiveManager()
    {
        if (Persistence)
        {
            
        }
    }
    
    public static IHiveManager GetInstance()
    {
        _instance ??= new HiveManager();
        return _instance;
    }

    public void AddHive(Guid id, Components.Hive hive)
    {
        hive.Persistent = Persistence;
        _instance.HiveStorage.TryAdd(id, hive);
    }

    public Components.Hive GetHive(Guid id) => _instance.HiveStorage[id];

    
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