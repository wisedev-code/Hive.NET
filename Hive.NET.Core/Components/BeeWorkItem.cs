using System.Diagnostics.Contracts;

namespace Hive.NET.Core.Components;

public class BeeWorkItem
{
    public BeeWorkItem(Task task, Action onSuccess = default, Action<Exception> onFailure = default)
    {
        this.task = task;
        this.onSuccess = onSuccess;
        this.onFailure = onFailure;
    }

    public Task task { get; }
    public Action onSuccess { get; }
    public Action<Exception> onFailure { get; }
}
