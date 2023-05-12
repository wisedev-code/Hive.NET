using System.Collections.Concurrent;
using Hive.NET.Core.Configuration;
using Hive.NET.Core.Models.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hive.NET.Core.Components;

public class Hive
{
    private ILogger<Hive> _logger;
    public Guid Id { get; }
    
    private List<Bee> Swarm = new();
    private ConcurrentQueue<BeeWorkItem> Tasks = new();
    private ConcurrentDictionary<Guid, BeeWorkItemStatus> Statuses = new(); 

    public Hive(int swarmSize = 3)
    {
        var options = ServiceLocator.GetService<IOptions<HiveSettings>>();
        _logger = BoostrapExtensions.BuildLogger<Hive>(options.Value.LogLevel);
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

        _logger.LogDebug($"Task {id} is removed from queue");
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
        
        _logger.LogDebug($"Task assigned to bee: {bee.Id}");
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
        _logger.LogInformation($"Bee {bee.Id} will perform task {workItem.Id}");
        var success = await bee.DoWork(workItem, callback);
        Statuses[workItem.Id] = success ? BeeWorkItemStatus.Completed : BeeWorkItemStatus.Failed;
    }
}