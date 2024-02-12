# Extensions Package Overview

The Hive Management Extension Package extends the functionality of hive management systems, providing additional features for managing BeeWorkItems and manipulating hive sequences efficiently. With these extensions, users can streamline task management within their hives and optimize task execution.


## Registration

The Bootstrapper class provides methods for registering API endpoints and SignalR connections, enhancing the hive management system with additional communication and monitoring capabilities.

- `AddHiveApi`: Registers the Hive API, allowing users to collect data from hives via HTTP endpoints.
- `AddHiveSignalR`: Registers SignalR connections for real-time notifications when events occur within the hives.

### Example

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddHiveApi();
    services.AddHiveSignalR();
}
```

### BeeWorkItemFluentBuilderExtensions

These extension methods enable the fluent construction of BeeWorkItems with specific attributes.

- `WithDescription`: Sets the description for the BeeWorkItem.
- `OnSuccess`: Specifies the action to be executed upon successful completion of the BeeWorkItem.
- `OnFailure`: Specifies the action to be executed upon failure of the BeeWorkItem.
- `WithNextTask`: Sets the next BeeWorkItem to be executed after the current one.

#### Example

```csharp
var task = new BeeWorkItemFluentBuilderExtensions()
    .WithDescription("Perform task")
    .OnSuccess(() => Console.WriteLine("Task completed successfully!"))
    .OnFailure((ex) => Console.WriteLine($"Task failed with exception: {ex.Message}"))
    .WithNextTask(new BeeWorkItem().WithDescription("Perform next task"));
```

### BeeWorkItemsSequenceExtensions

These extensions allow manipulation of sequences of BeeWorkItems.

- `AddWorkItem`: Adds a single BeeWorkItem to the sequence.
- `AddWorkItems`: Adds multiple BeeWorkItems to the sequence.
- `RemoveWorkItem`: Removes a BeeWorkItem from the sequence.

#### Example
```csharp
    var sequence = new BeeWorkItemsSequence()
        .AddWorkItem(new BeeWorkItem().WithDescription("Task 1"))
        .AddWorkItem(new BeeWorkItem().WithDescription("Task 2"))
        .RemoveWorkItem(sequence[0]);
```

### HiveExtensions

These extensions provide additional functionality to the Hive class.

- `HasFreeBees`: Checks if there are bees available in the hive that are not currently working.
- `AddTaskWithPriority`: Adds a BeeWorkItem to the hive with a specified priority, influencing the order of execution.

#### Example
```csharp
    var hive = hiveManager.GetHive(id);
    if (hive.HasFreeBees())
    {
        var task = new BeeWorkItem().WithDescription("Perform high-priority task");
        hive.AddTaskWithPriority(task, BeeWorkItemPriority.High);

        var lowPriorityTask = new BeeWorkItem().WithDescription("Perform low-priority task");
        hive.AddTaskWithPriority(lowPriorityTask, BeeWorkItemPriority.Low);
    }
```

