using Hive.NET.Core.Manager;

namespace Hive.NET.Core.Factory;

public static class HiveFactory
{
    public static Guid CreateHive(int swarmSize)
    {
        var newHive = new Components.Hive(swarmSize);

        IHiveManager manager = HiveManager.GetInstance();
        
        manager.AddHive(newHive.Id, newHive);

        return newHive.Id;
    }
}