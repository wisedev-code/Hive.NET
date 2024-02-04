using System.Collections.Concurrent;
using Hive.NET.Core.Components;
using Hive.NET.Core.Models.Enums;

namespace Hive.NET.Extensions.Components;

public static class HiveExtensions
{
    public static bool HasFreeBees(this Core.Components.Hive hive)
    {
        return hive.Swarm.Any(bee => !bee.IsWorking);
    }
    
    public static Guid AddTaskWithPriority(this Core.Components.Hive hive, BeeWorkItem task, BeeWorkItemPriority priority)
    {
        var taskId = hive.AddTask(task);
        hive.Statuses[taskId] = (WorkItemStatus.Waiting, DateTime.UtcNow, (int)priority);
        hive.Tasks = new ConcurrentQueue<BeeWorkItem>(
            hive.Tasks.OrderBy(t => hive.Statuses[t.Id].Priority)
        );
        hive.AssignTaskToRandomBee();
        return taskId;
    }
}