using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Hive.NET.Core.Components;

public class BeeWorkItem
{
    public BeeWorkItem(Task task, string? description = null, Action onSuccess = default, Action<Exception> onFailure = default)
    {
        this.task = task;
        this.onSuccess = onSuccess;
        this.onFailure = onFailure;
    }

    public Task task { get; }
    public Action onSuccess { get; }
    public Action<Exception> onFailure { get; }
    public Guid Id { get; set; }
    public string Description { get; set; }
}
