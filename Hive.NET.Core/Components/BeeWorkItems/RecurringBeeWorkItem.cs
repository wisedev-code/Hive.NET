using System;
using System.Threading.Tasks;

namespace Hive.NET.Core.Components.BeeWorkItems;

public class RecurringBeeWorkItem
{
    public RecurringBeeWorkItem(Action task, 
        string? description = null,  
        Action? onSuccess = default, 
        Action<Exception>? onFailure = default)
    {
        Id = Guid.NewGuid();
        Description = description;
        this.onSuccess = onSuccess;
        this.onFailure = onFailure;
        Task = task;
    }

    public Guid Id { get; set; }
    public string? Description { get; set; }
    public Action? onSuccess { get; }
    public Action<Exception>? onFailure { get; }
    public Action Task { get; set; }

    public BeeWorkItem CreateBeeWorkItem() => new(new Task(action: Task), Description, onSuccess, onFailure);
}