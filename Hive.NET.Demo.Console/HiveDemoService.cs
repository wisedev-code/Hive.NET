using Hive.NET.Core.Components;
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
        var hive = new Core.Components.Hive(2);

        System.Console.Write("Set amount of tasks to process or leave empty to quit: ");
        
        while (true)
        {
            var input = System.Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                break;
            }

            if (int.TryParse(input, out var amount))
            {
                var tasks = CreateTasks(amount);
                
                tasks.ForEach(x => hive.AddTask(x));
            }

        }
    }

    private List<Task> CreateTasks(int amount)
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