using System.Collections.Concurrent;
using Hive.NET.Core.Models.Enums;

namespace Hive.NET.Core.Components;

public class Hive
{
    public Guid Id { get; }
    
    private List<Bee> Swarm = new();
    private ConcurrentQueue<BeeWorkItem> Tasks = new();
    private ConcurrentDictionary<Guid, BeeWorkItemStatus> Statuses = new(); 

    public Hive(int swarmSize = 3)
    {
        Id = Guid.NewGuid();
        
        for (int i = 0; i < swarmSize; i++)
        {
            Swarm.Add(new Bee());
        }
    }

    public Guid AddTask(BeeWorkItem task)
    {
        var taskId = Guid.NewGuid();
        Statuses.TryAdd(taskId, BeeWorkItemStatus.Waiting);
        task.Id = taskId;
        Tasks.Enqueue(task);
        AssignTaskToRandomBee();
        
        return taskId;
    }

    public BeeWorkItemStatus GetWorkItemStatus(Guid id)
    {
        if (!Statuses.ContainsKey(id))
        {
            return BeeWorkItemStatus.NotExist;
        }

        return Statuses[id];
    }
    
    public BeeWorkItemStatus TryRemoveTask(Guid id)
    {
        if (!Statuses.ContainsKey(id))
        {
            return BeeWorkItemStatus.NotExist;
        }

        if (Statuses[id] == BeeWorkItemStatus.Running)
        {
            //todo Log error that cannot remove running Task
            return BeeWorkItemStatus.Running;
        }

        return Statuses[id] = BeeWorkItemStatus.Removed;
    }

    private async Task AssignTaskToBee(Bee bee)
    {
        Bee.BeeFinishedWorkCallback callback = AssignTaskToBee;

        if (Tasks.TryDequeue(out var task))
        {
            var state = GetWorkItemStatus(task.Id);
            if (state == BeeWorkItemStatus.Removed)
            {
                return;
            }
            
            Statuses[Id] = BeeWorkItemStatus.Running;
            await InvokeTaskOnBee(bee, task, callback);
        }
    }

    private async Task AssignTaskToRandomBee()
    {
        Bee.BeeFinishedWorkCallback callback = AssignTaskToBee;
        
        var bee = Swarm.FirstOrDefault(x => x.IsWorking == false);
        
        if (bee is null)
        {
            return;
        }
        
        if (Tasks.TryDequeue(out var task))
        {
            var state = GetWorkItemStatus(task.Id);
            if (state == BeeWorkItemStatus.Removed)
            {
                return;
            }
            
            await InvokeTaskOnBee(bee, task, callback);
        }
    }


    private async Task InvokeTaskOnBee(Bee bee, BeeWorkItem workItem, Bee.BeeFinishedWorkCallback callback)
    {
        var success = await bee.DoWork(workItem, callback);
        Statuses[workItem.Id] = success ? BeeWorkItemStatus.Completed : BeeWorkItemStatus.Failed;
    }
}