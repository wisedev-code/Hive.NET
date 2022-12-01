namespace Hive.NET.Core.Manager;

public interface IHiveManager
{
    void AddHive(Guid id, Components.Hive hive);
    Components.Hive GetHive(Guid id);
}