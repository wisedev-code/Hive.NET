using System;
using System.Threading.Tasks;

namespace Hive.NET.Core.Components;

public class BeeWorkItem
{
    public BeeWorkItem(
        Task task, 
        string? description = null,
        Action onSuccess = default, 
        Action<Exception> onFailure = default)
    {
        this.task = task;
        this.onSuccess = onSuccess;
        this.onFailure = onFailure;
    }

    internal BeeWorkItem(Guid id, string? description)
    {
        Id = id;
        Description = description;
    }

    public Task task { get; }
    public Action onSuccess { get; internal set; }
    public Action<Exception> onFailure { get; internal set; }
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public BeeWorkItem? NextTask { get; private set; }
    public void AddNextTask(BeeWorkItem nextTask)
        => NextTask = nextTask;
}
