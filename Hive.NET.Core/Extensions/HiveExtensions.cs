using System;
using System.Linq;
using System.Timers;
using Hive.NET.Core.Components;
using Hive.NET.Core.Components.BeeWorkItems;
using Hive.NET.Core.Models.Enums;

namespace Hive.NET.Core.Extensions;

public static class HiveExtensions
{
    public static Guid AddTask(this Components.Hive hive, RecurringBeeWorkItem recurringTask, TimeSpan interval)
    {
        var taskId = recurringTask.Id;
        // Schedule the initial execution
        var nextExecutionTime = DateTime.UtcNow.Add(interval);
        hive.Statuses.TryAdd(taskId, (WorkItemStatus.Waiting, nextExecutionTime));
        
        // Create a timer for the recurring task
        var timer = new System.Timers.Timer();
        timer.Interval = interval.TotalMilliseconds;
        timer.Elapsed += (sender, args) => DoWork(hive, recurringTask, sender as Timer);
        timer.AutoReset = true;
        timer.Start();

        DoWork(hive, recurringTask, null);
        return taskId;
    }

    private static void DoWork(Components.Hive hive, RecurringBeeWorkItem workItem, Timer? sender)
    {
        var currentState = hive.Statuses[workItem.Id];
        if (currentState.Status == WorkItemStatus.Removed && sender != null)
        {
            sender.Close();
            return;
        }
        
        var beeWorkItem = workItem.CreateBeeWorkItem();
        beeWorkItem.Id = workItem.Id;
        Bee.BeeFinishedWorkCallback callback = hive.AssignTaskToBee;
        hive.Statuses[workItem.Id] = new (WorkItemStatus.Running, DateTime.UtcNow);
        hive.InvokeTaskOnBee(hive.Swarm.FirstOrDefault(x => x.IsWorking == false), beeWorkItem, callback);
    }
}