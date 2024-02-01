using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hive.NET.Core.Api;
using Hive.NET.Core.Configuration;
using Hive.NET.Core.Configuration.Storage;
using Hive.NET.Core.Models.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hive.NET.Core.Components;

public class Hive
{
    private ILogger<Hive> _logger;
    public Guid Id { get; init; }
    internal string _name;
    internal List<Bee> Swarm = new();
    private ConcurrentQueue<BeeWorkItem> Tasks = new();
    internal List<BeeWorkItem> Items = new();
    internal ConcurrentDictionary<Guid, (WorkItemStatus Status, DateTime UpdatedAt)> Statuses = new();

    private readonly bool _persistent;
    private readonly IHiveStorageProvider _hiveStorageProvider;

    public Hive(int swarmSize = 3, string? name = null)
    {
        if (name == null)
        {
            _name = Guid.NewGuid().ToString(); //todo create fun animal names by default.
        }
        else
        {
            _name = name;
        }
        
        var options = ServiceLocator.GetService<IOptions<HiveSettings>>();
        _logger = BoostrapExtensions.BuildLogger<Hive>(options.Value.LogLevel);
        _persistent = options.Value.Persistence; 
        Id = Guid.NewGuid();
        
        for (int i = 0; i < swarmSize; i++)
        {
            Swarm.Add(new Bee());
        }

        _hiveStorageProvider = ServiceLocator.GetService<IHiveStorageProvider>();
    }

    public Guid AddTask(BeeWorkItem task)
    {
        var taskId = Guid.NewGuid();
        Statuses.TryAdd(taskId, (WorkItemStatus.Waiting, DateTime.UtcNow));
        task.Id = taskId;
        Tasks.Enqueue(task);
        Items.Add(task);
        AssignTaskToRandomBee();
        
        return taskId;
    }
    
    public Guid AddTask(BeeWorkItemsSequence task)
    {
        var items = task.All().ToList();
        if (items.Count == 0)
        {
            throw new InvalidOperationException("WorkItemSequence does not contain elements");
        }
        
        BeeWorkItem firstInSequence = items.First();
        BeeWorkItem previousItem = firstInSequence;

        foreach (var item in items.Skip(1))
        {
            previousItem.AddNextTask(item);
            previousItem = item;
        }
        
        Statuses.TryAdd(firstInSequence.Id, (WorkItemStatus.Waiting, DateTime.UtcNow));
        task.Id = firstInSequence.Id;
        Tasks.Enqueue(firstInSequence);
        Items.Add(firstInSequence);
        AssignTaskToRandomBee();
        
        return task.Id;
    }

    public WorkItemStatus GetWorkItemStatus(Guid id)
    {
        if (!Statuses.ContainsKey(id))
        {
            return WorkItemStatus.NotExist;
        }

        return Statuses[id].Status;
    }
    
    public WorkItemStatus WorkItemStatusTryRemoveTask(Guid id)
    {
        if (!Statuses.ContainsKey(id))
        {
            return WorkItemStatus.NotExist;
        }

        if (Statuses[id].Status == WorkItemStatus.Running)
        {
            //todo Log error that cannot remove running Task
            return WorkItemStatus.Running;
        }

        _logger.LogDebug($"Task {id} is removed from queue");
        return (Statuses[id] = new (WorkItemStatus.Removed, DateTime.Now)).Status;
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
            
            Statuses[Id] = new (
                WorkItemStatus.Running, DateTime.UtcNow);
            
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
        Statuses[workItem.Id] = success ? new (WorkItemStatus.Completed, DateTime.UtcNow)
            : new (WorkItemStatus.Failed, DateTime.UtcNow);

        if (_persistent)
        {
            _hiveStorageProvider.UpsertHive(Id, this);
        }
    }

    internal HiveDto MapToDto() =>
        new()
        {
            Id = Id,
            Name = _name,
            Bees = Swarm.Select(x => new BeeDto
            {
                Id = x.Id,
                IsWorking = x.IsWorking
            }).ToList()
        };

    internal HiveDetailsDto MapToDetailsDto() =>
        new()
        {
            Id = Id,
            Name = _name,
            Swarm = Swarm.Select(x => new BeeDto()
            {
                Id = x.Id,
                IsWorking = x.IsWorking
            }).ToList(),
            WorkItems = Items.Select(workItem =>
                new BeeWorkItemDto()
                {
                    Id = workItem.Id,
                    Status = Statuses[workItem.Id].Status,
                    UpdatedAt = Statuses[workItem.Id].UpdatedAt,
                    Description = workItem.Description
                }).ToList()
        };
    
    internal List<BeeErrorDto> MapToErrorsDto() =>
        Swarm.SelectMany(bee =>
            bee.RegisteredErrors.Select(x => x.MapToDto())).ToList();
}