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

    public static HiveManager GetInstance()
    {
        _instance ??= new HiveManager();
        return _instance;
    }

    public void AddHive(Guid id, Components.Hive hive) => _instance.HiveStorage.TryAdd(id,hive);
    public Components.Hive GetHive(Guid id) => _instance.HiveStorage[id];
    public List<HiveDto> GetHives()
    {
        return HiveStorage.Values.Select(x => new HiveDto
        {
            Id = x.Id,
            Name = x.Name,
            Bees = x.Swarm.Select(x => new BeeDto
            {
                Id = x.Id,
                IsWorking = x.IsWorking
            }).ToList()
        }).ToList();
    }
}