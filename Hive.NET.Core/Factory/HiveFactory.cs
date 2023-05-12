using Hive.NET.Core.Configuration;
using Hive.NET.Core.Manager;
using Microsoft.Extensions.Logging;

namespace Hive.NET.Core.Factory;

public static class HiveFactory
{
    private static ILogger<Components.Hive> _logger;
    
    public static Guid CreateHive(int swarmSize)
    {
        _logger = BoostrapExtensions.BuildLogger<Components.Hive>(LogLevel.Information);
        var newHive = new Components.Hive(swarmSize);

        IHiveManager manager = HiveManager.GetInstance();
        
        manager.AddHive(newHive.Id, newHive);
        
        _logger.LogInformation($"Created Hive with Id: {newHive.Id}, and {swarmSize} bees inside");
        return newHive.Id;
    }
}