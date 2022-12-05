using System.Collections.Concurrent;

namespace Hive.NET.Core.Components;

public class Hive
{
    private List<Bee> Swarm = new();
    private ConcurrentQueue<Task> Tasks = new();


    public Hive(int swarmSize = 3)
    {
        for (int i = 0; i < swarmSize; i++)
        {
            Swarm.Add(new Bee());
        }
    }

    public void AddTask(Task task)
    {
        Tasks.Enqueue(task);
        
        AssignTask();
    }

    private void AssignTaskToBee(Bee bee)
    {
        Bee.BeeFinishedWorkCallback callback = AssignTaskToBee;

        if (Tasks.TryDequeue(out var task))
        {
            bee.DoWork(task, callback);
        }
    }

    private void AssignTask()
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