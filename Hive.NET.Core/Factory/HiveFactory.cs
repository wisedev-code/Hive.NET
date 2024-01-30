using System;
using Hive.NET.Core.Configuration;
using Hive.NET.Core.Manager;
using Microsoft.Extensions.Logging;

namespace Hive.NET.Core.Factory;

public static class HiveFactory
{
    private static ILogger<Components.Hive> _logger;
    
    public static Guid CreateHive(int swarmSize, string name = "")
    {
        IHiveManager manager = HiveManager.GetInstance();
        _logger = BoostrapExtensions.BuildLogger<Components.Hive>(LogLevel.Information);
        var newHive = manager.GetHive(name) ?? new Components.Hive(swarmSize, name);
        manager.AddHive(newHive.Id, newHive);
        
        _logger.LogInformation($"Created Hive with Id: {newHive.Id}, and {swarmSize} bees inside");
        return newHive.Id;
    }
}