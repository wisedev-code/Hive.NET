using Hive.NET.Core.Components;
using Hive.NET.Core.Factory;
using Hive.NET.Core.Manager;
using Microsoft.Extensions.Logging;

namespace Hive.NET.Demo.Console;

public class HiveDemoService
{
    private readonly IHiveManager _manager;
    private readonly ILogger<HiveDemoService> _logger;

    public HiveDemoService(IHiveManager manager,
        ILogger<HiveDemoService> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation("Select test to run:" + Environment.NewLine +
                               "1 Normal Tasks " + Environment.NewLine +
                               "2 Sequential task");

        switch (System.Console.ReadLine())
        {
            case "1": RunNormalTasks();
                break;
            case "2": RunSequentialTasks();
                break;
        }
    }

    private void RunNormalTasks()
    {
        var hiveId = HiveFactory.CreateHive(2);
        var hive = _manager.GetHive(hiveId);

        _logger.LogInformation("Set amount of tasks to process or leave empty to quit: ");

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

            tasks.ForEach(x => hive.AddTask(new BeeWorkItem(x, string.Empty, () => System.Console.WriteLine("Finished!"))));
        }
    }

    private void RunSequentialTasks()
    {
        var hiveId = HiveFactory.CreateHive(2);

        var hive = _manager.GetHive(hiveId);

        var seqTasks = CreateTasks(3);

        var beeTaskSeq1 = new BeeWorkItem(seqTasks[0], () => System.Console.WriteLine("Finished 1-1!"));
        var beeTaskSeq2 = new BeeWorkItem(seqTasks[1], () => System.Console.WriteLine("Finished 1-2!"));
        var beeTaskSeq3 = new BeeWorkItem(seqTasks[2], () => System.Console.WriteLine("Finished 1-3!"));

        beeTaskSeq1.AddNextTask(beeTaskSeq2);
        beeTaskSeq2.AddNextTask(beeTaskSeq3);

        hive.AddTask(beeTaskSeq1);

        var tasks = CreateTasks(5);
        tasks.ForEach(x => hive.AddTask(new BeeWorkItem(x, () => System.Console.WriteLine("Finished!"))));

        System.Console.ReadLine();
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
                Task.Delay(delay).Wait();
                System.Console.WriteLine($"Task {i1} with delay {delay}");
            }));
        }

        return tasks;
    }
}