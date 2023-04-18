using Hive.NET.Core.Components;
using Hive.NET.Core.Factory;
using Hive.NET.Core.Manager;

namespace Hive.NET.Demo.Console;

public class HiveDemoService
{
    private readonly IHiveManager _manager;

    public HiveDemoService(IHiveManager manager)
    {
        _manager = manager;
    }

    public void Run()
    {
        var hiveId = HiveFactory.CreateHive(2);

        var hive = _manager.GetHive(hiveId);
        
        
        System.Console.Write("Set amount of tasks to process or leave empty to quit: ");
        
        while (true)
        {
            var input = System.Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                break;
            }

            if (!int.TryParse(input, out var amount))
            {
                continue;;
            }

            var tasks = CreateTasks(amount);

            tasks.ForEach(x => hive.AddTask(new BeeWorkItem(x, () => System.Console.WriteLine("Finished!"))));

        }
    }

    private static List<Task> CreateTasks(int amount)
    {
        var tasks = new List<Task>();

        for (int i = 0; i < amount; i++)
        {
            var i1 = i+1;
            tasks.Add(new Task(() =>
            {
                var delay = new Random().Next(1000, 5000);
                System.Console.WriteLine($"Task {i1} with delay {delay}");
                Task.Delay(delay).Wait();
            }));
        }

        return tasks;
    }
}