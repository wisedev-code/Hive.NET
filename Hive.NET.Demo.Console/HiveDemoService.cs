using Hive.NET.Core.Manager;

namespace Hive.NET.Demo.Console;

public class HiveDemoService
{
    private readonly IHiveManager _manager;

    public HiveDemoService(IHiveManager manager)
    {
        _manager = manager;
    }

    public async Task Run()
    {
        var id = Guid.NewGuid();
        var hiveToAdd = new Core.Components.Hive();

        _manager.AddHive(id, hiveToAdd);


        var hive = _manager.GetHive(id);

        for (int i = 0; i < 20; i++)
        {
            var i1 = i;
            hive.AddTask(Guid.NewGuid(), new Task(() =>
            {
                System.Console.WriteLine($"Task {i1}");
                Task.Delay(4000);
            }));
        }
    }
}