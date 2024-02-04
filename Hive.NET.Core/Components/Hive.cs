using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hive.NET.Core.Api;
using Hive.NET.Core.Components.BeeWorkItems;
using Hive.NET.Core.Configuration;
using Hive.NET.Core.Configuration.Notification;
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
    internal ConcurrentQueue<BeeWorkItem> Tasks = new();
    internal List<BeeWorkItem> Items = new();
    internal ConcurrentDictionary<Guid, (WorkItemStatus Status, DateTime UpdatedAt, int Priority)> Statuses = new();

    private readonly bool _persistent;
    private readonly IHiveStorageProvider _hiveStorageProvider;
    private readonly INotificationProvider _notificationProvider;

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
        _notificationProvider = ServiceLocator.GetService<INotificationProvider>();
    }

    public Guid AddTask(BeeWorkItem task)
    {
        var taskId = Guid.NewGuid();
        Statuses.TryAdd(taskId, (WorkItemStatus.Waiting, DateTime.UtcNow, Priority: 2));
        task.Id = taskId;
        
        Tasks.Enqueue(task);
        //reorder to keep prio in sync
        Tasks = new ConcurrentQueue<BeeWorkItem>(
            Tasks.OrderBy(t => Statuses[t.Id].Priority)
        );
        
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
        
        Statuses.TryAdd(firstInSequence.Id, (WorkItemStatus.Waiting, DateTime.UtcNow, (int)BeeWorkItemPriority.Medium));
        task.Id = firstInSequence.Id;
        Tasks.Enqueue(firstInSequence);
        Items.Add(firstInSequence);
        AssignTaskToRandomBee();
        
        return task.Id;
    }

    public (WorkItemStatus Status, DateTime UpdatedAt, int Priority) GetWorkItemStatus(Guid id)
    {
        if (!Statuses.ContainsKey(id))
        {
            return new ValueTuple<WorkItemStatus, DateTime, int>(WorkItemStatus.NotExist, DateTime.MinValue, (int)BeeWorkItemPriority.VeryLow);
        }

        return Statuses[id];
    }
    
    public WorkItemStatus TryRemoveTask(Guid id)
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
        return (Statuses[id] = new (WorkItemStatus.Removed, DateTime.Now, (int)BeeWorkItemPriority.Medium)).Status;
    }

    internal async Task AssignTaskToBee(Bee bee)
    {
        Bee.BeeFinishedWorkCallback callback = AssignTaskToBee;

        if (Tasks.TryDequeue(out var task))
        {
            var state = GetWorkItemStatus(task.Id);
            if (state.Status == WorkItemStatus.Removed)
            {
                return;
            }
            
            Statuses[Id] = new (
                WorkItemStatus.Running, DateTime.UtcNow, state.Priority);
            
            await InvokeTaskOnBee(bee, task, callback);
        }
    }

    internal async Task AssignTaskToRandomBee()
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
            if (state.Status == WorkItemStatus.Removed)
            {
                return;
            }
            
            await InvokeTaskOnBee(bee, task, callback);
        }
    }


    internal async Task InvokeTaskOnBee(Bee bee, BeeWorkItem workItem, Bee.BeeFinishedWorkCallback callback)
    {
        _logger.LogInformation($"Bee {bee.Id} will perform task {workItem.Id}");
        var success = await bee.DoWork(workItem, callback);
        var currentState = Statuses[workItem.Id];
        Statuses[workItem.Id] = success ? new (WorkItemStatus.Completed, DateTime.UtcNow, currentState.Priority)
            : new (WorkItemStatus.Failed, DateTime.UtcNow, currentState.Priority);
        
        _notificationProvider.Notify(new
        {
            ItemId = workItem.Id,
            ItemDescription = workItem.Description,
            Status = Statuses[workItem.Id].Status,
            UpdatedAt = Statuses[workItem.Id].UpdatedAt
        });

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