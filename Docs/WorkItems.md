# BeeWorkItem

The `BeeWorkItem` class represents a work item that can be processed by bees within the hive system. It encapsulates a task to be executed, along with optional callbacks for success and failure handling. Users interact with `BeeWorkItem` instances primarily through the hive, as direct manipulation is restricted to ensure consistency and control over task execution.

## Constructor:

The `BeeWorkItem` constructor allows users to create instances of work items, specifying the task to be executed and optional callbacks for success and failure handling. The constructor signature is as follows:

```csharp
public BeeWorkItem(
    Task task,
    string? description = null,
    Action onSuccess = null,
    Action<Exception> onFailure = null
)
```

- *task*: The task to be executed by the work item. This parameter is mandatory and must be provided by the user.
- *description*: An optional description of the work item, providing additional context or information about its purpose.
- *onSuccess*: An optional callback to be executed upon successful completion of the task.
- *onFailure*: An optional callback to be executed if the task encounters an exception or fails to execute.

## BeeWorkItem Functionalities

### Types of BeeWorkItem:

1. **Single BeeWorkItem:**
   - Represents a standalone task to be executed by a single bee within the hive. Users create instances of single `BeeWorkItem` to perform specific actions or operations within the hive.

2. **Sequence BeeWorkItem:**
   - Consists of a sequence of interconnected tasks, where each task depends on the successful completion of its predecessor. Sequence `BeeWorkItem` allows users to define complex workflows and dependencies within the hive.

3. **Recurring BeeWorkItem:**
   - Represents a task that is scheduled to be executed periodically at specified intervals. Recurring `BeeWorkItem` instances enable users to automate repetitive tasks and maintenance operations within the hive.

### Interacting with BeeWorkItem:

Users interact with `BeeWorkItem` instances primarily through the hive, which manages their execution and status. While direct manipulation of `BeeWorkItem` instances is restricted, users can:

- Add `BeeWorkItem` instances to the hive for processing.
- Retrieve the status of a `BeeWorkItem`'s processing through the hive, providing insight into its execution progress and outcome.
- Utilize the `NextTask` field within `BeeWorkItem` instances for running tasks recursively, enabling sequential execution of dependent tasks.


## Examples:

### Creating Single BeeWorkItem:
```csharp
var singleTask = new Task(() => Console.WriteLine("Single task executed"));
var singleBeeWorkItem = new BeeWorkItem(singleTask, "Description of single task");
```

### Creating Sequence BeeWorkItem:
```csharp
var task1 = new Task(() => Console.WriteLine("Task 1 executed"));
var task2 = new Task(() => Console.WriteLine("Task 2 executed"));
var sequenceBeeWorkItem = new BeeWorkItemsSequence("Sequence Description");
sequenceBeeWorkItem.AddWorkItem(new BeeWorkItem(task1));
sequenceBeeWorkItem.AddWorkItem(new BeeWorkItem(task2));

```


### Creating Recurring BeeWorkItem:
```csharp
var recurringTask = new Task(() => Console.WriteLine("Recurring task executed"));
var recurringBeeWorkItem = new RecurringBeeWorkItem(recurringTask, "Description of recurring task");


```