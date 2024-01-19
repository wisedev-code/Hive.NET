using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Hive.NET.Core.Configuration;
using Hive.NET.Core.Models.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hive.NET.Core.Components;

public class Hive
{
    private ILogger<Hive> _logger;
    public Guid Id { get; }
    internal string Name;
    internal List<Bee> Swarm = new();
    private ConcurrentQueue<BeeWorkItem> Tasks = new();
    private ConcurrentDictionary<Guid, WorkItemStatus> Statuses = new(); 

    public Hive(int swarmSize = 3, string? name = null)
    {
        if (name == null)
        {
            Name = Guid.NewGuid().ToString(); //todo create fun animal names by default.
        }
        
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
        Statuses.TryAdd(taskId, WorkItemStatus.Waiting);
        task.Id = taskId;
        Tasks.Enqueue(task);
        AssignTaskToRandomBee();
        
        return taskId;
    }

    public WorkItemStatus GetWorkItemStatus(Guid id)
    {
        if (!Statuses.ContainsKey(id))
        {
            return WorkItemStatus.NotExist;
        }

        return Statuses[id];
    }
    
    public WorkItemStatus WorkItemStatusTryRemoveTask(Guid id)
    {
        if (!Statuses.ContainsKey(id))
        {
            return WorkItemStatus.NotExist;
        }

        if (Statuses[id] == WorkItemStatus.Running)
        {
            //todo Log error that cannot remove running Task
            return WorkItemStatus.Running;
        }

        _logger.LogDebug($"Task {id} is removed from queue");
        return Statuses[id] = WorkItemStatus.Removed;
    }

    private async Task AssignTaskToBee(Bee bee)
    {
        Bee.BeeFinishedWorkCallback callback = AssignTaskToBee;

        if (Tasks.TryDequeue(out var task))
        {
            var state = GetWorkItemStatus(task.Id);
            if (state == WorkItemStatus.Removed)
            {
                return;
            }
            
            Statuses[Id] = WorkItemStatus.Running;
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
            if (state == WorkItemStatus.Removed)
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
        Statuses[workItem.Id] = success ? WorkItemStatus.Completed : WorkItemStatus.Failed;
    }
}