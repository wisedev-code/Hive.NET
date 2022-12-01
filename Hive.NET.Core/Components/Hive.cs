using System.Collections.Concurrent;

namespace Hive.NET.Core.Components;

public class Hive
{
    private List<Bee> Swarm = new();
    private ConcurrentDictionary<Guid, Task> Tasks = new();


    public Hive(int amount = 3)
    {
        for (int i = 0; i < amount; i++)
        {
            Swarm.Add(new Bee());
        }

        Thread thread = new Thread(OperateHive);
        thread.Start();

        //new Timer((e) => { OperateHive(); }, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
    }

    public void AddTask(Guid id, Task task)
    {
        Tasks[id] = task;
    }

    private void OperateHive()
    {
        while (true)
        {
            if (Tasks.Count is 0)
            {
                Console.WriteLine("No tasks found");
                Thread.Sleep(5000);
                continue;
            }

            var bee = Swarm.FirstOrDefault(x => x.IsWorking == false);

            if (bee is null)
            {
                Console.WriteLine("Waiting for free bee");
                Thread.Sleep(1000);
                continue;
            }

            var task = Tasks.FirstOrDefault();
            Tasks.Remove(task.Key, out _);
            Console.WriteLine($"Bee {bee.Id} is starting work");
            bee.DoWork(task.Value);
        }
    }
}