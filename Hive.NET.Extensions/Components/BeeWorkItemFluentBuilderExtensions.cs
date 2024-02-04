using Hive.NET.Core.Components;

namespace Hive.NET.Extensions.Components;

public static class BeeWorkItemFluentBuilderExtensions
{
    public static BeeWorkItem WithDescription(this BeeWorkItem workItem, string description)
    {
        workItem.Description = description;
        return workItem;
    }

    public static BeeWorkItem OnSuccess(this BeeWorkItem workItem, Action onSuccess)
    {
        workItem.onSuccess = onSuccess;
        return workItem;
    }

    public static BeeWorkItem OnFailure(this BeeWorkItem workItem, Action<Exception> onFailure)
    {
        workItem.onFailure = onFailure;
        return workItem;
    }

    public static BeeWorkItem WithNextTask(this BeeWorkItem workItem, BeeWorkItem nextTask)
    {
        workItem.AddNextTask(nextTask);
        return workItem;
    }
}