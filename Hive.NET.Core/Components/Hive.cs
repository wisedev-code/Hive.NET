using System.Collections.Concurrent;

namespace Hive.NET.Core.Components;

public class Hive
{
    public Guid Id { get; }
    
    private List<Bee> Swarm = new();
    private ConcurrentQueue<BeeWorkItem> Tasks = new();

    public Hive(int swarmSize = 3)
    {
        Id = Guid.NewGuid();
        
        for (int i = 0; i < swarmSize; i++)
        {
            Swarm.Add(new Bee());
        }
    }

    public void AddTask(BeeWorkItem task)
    {
        Tasks.Enqueue(task);
        
        AssignTaskToRandomBee();
    }

    private void AssignTaskToBee(Bee bee)
    {
        Bee.BeeFinishedWorkCallback callback = AssignTaskToBee;

        if (Tasks.TryDequeue(out var task))
        {
            bee.DoWork(task, callback);
        }
    }

    private void AssignTaskToRandomBee()
    {
        Bee.BeeFinishedWorkCallback callback = AssignTaskToBee;
        
        var bee = Swarm.FirstOrDefault(x => x.IsWorking == false);
        
        if (bee is null)
        {
            return;
        }
        
        if (Tasks.TryDequeue(out var task))
        {
            bee.DoWork(task, callback);
        }
    }
}