# Hive

### Table of Contents
- [Hive overview](#hive-documentation)
  - [Functionalities](#functionalities)
    1. [Adding a Task](#adding-a-task)
    2. [Adding a Sequence of Tasks](#adding-a-sequence-of-tasks)
    3. [Adding Recurring Tasks](#adding-recurring-tasks)
    4. [Getting Task Status](#getting-task-status)
    5. [Removing a Task](#removing-a-task)
    6. [Clearing Hive](#clearing-hive)

## Overview 

The `Hive` class represents a centralized entity where bees collaborate to execute tasks efficiently. Conceptually mirroring a real beehive, this class serves as a hub for task management within an application, facilitating parallel processing and task delegation.

Within the `Hive`, tasks are managed in a queue-based system, ensuring that bees can efficiently pick up and execute tasks as they become available. This architecture enables users to define complex workflows by adding individual tasks or sequences of tasks to the hive. Each task is executed asynchronously by a bee, allowing for concurrent processing and optimal resource utilization.

Furthermore, the `Hive` class provides functionalities to monitor and manage the execution of tasks. Users can retrieve the status of specific tasks within the hive, enabling real-time monitoring of task progress. Additionally, tasks can be removed from the hive if necessary, providing flexibility in managing task execution. Users can also add recurring tasks to the hive, which are executed at specified intervals, enhancing automation capabilities and task scheduling.

**Creating a Hive:**
   - Users can create a hive by calling the `HiveFactory.CreateHive()` method and specifying the desired swarm size (number of bees) as a parameter. This method returns a unique identifier (hiveId) for the newly created hive, which can then be used to access and manipulate the hive.

Overall, the `Hive` class serves as a versatile tool for orchestrating and optimizing task execution within applications, enhancing performance and scalability.

## Functionalities:

1. ### Adding a Task: ###
   - Allows users to add a task to the hive, which will be processed by bees. Tasks can be added individually or as part of a sequence.
   - Example:
     ```csharp
     var hiveId = HiveFactory.CreateHive(2);
     var hiveManager = HiveManager.GetInstance();
     var task = new BeeWorkItem(new Task(() => Console.WriteLine("Task executed")));
     var taskId = hiveManager
        .GetHive(hiveId)
        .AddTask(task);
     ```

2. ### Adding a Sequence of Tasks: ###
   - Provides functionality to add a sequence of tasks to the hive, ensuring their execution in order. This allows users to create complex workflows for the hive to execute.
   - Example:
     ```csharp
     var hiveId = HiveFactory.CreateHive(2);
     var hiveManager = HiveManager.GetInstance();
     
     var task1 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 1 executed")));
     var task2 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 2 executed")));
     var sequence = new BeeWorkItemsSequence("Sequence Description");

     sequence.AddWorkItem(task1);
     sequence.AddWorkItem(task2);

     hiveManager
        .GetHive(hiveId)
        .AddTask(sequence);
     ```

3. ### Adding Recurring Tasks: ###
   - Users can add a recurring task to the hive by specifying the task to execute and the interval at which it should be repeated. This functionality enhances automation capabilities and task scheduling within the application.
   - Example:
     ```csharp
     var hiveId = HiveFactory.CreateHive(2);
     var hiveManager = new HiveManager.GetInstance()
     var recurringTask = new RecurringBeeWorkItem(
        () => Console.WriteLine("Recurring Task"), 
     "Task description", 
     () => Console.WriteLine("success"), 
     () => Console.WriteLine("failure");
     hiveManager
        .GetHive(hiveId)
        .AddTask(recurringTask, TimeSpan.FromMinutes(5));
     ```

4. ### Getting Task Status: ###
   - Enables users to retrieve the status of a specific task within the hive. Users can check whether a task is waiting, running, completed, failed, or removed.
   - Example:
     ```csharp
     var hiveId = HiveFactory.CreateHive(2);
     var hiveManager = HiveManager.GetInstance()
     var taskId = Guid.NewGuid(); 
     var taskStatus = hiveManager
        .GetHive(hiveId)
        .GetWorkItemStatus(taskId);
     ```

5. ### Removing a Task: ###
   - Allows users to remove a task from the hive, if needed. This functionality is useful when a task needs to be canceled or removed from the hive's queue.
   - Example:
     ```csharp
     var hiveId = HiveFactory.CreateHive(2);
     var hiveManager = HiveManager.GetInstance()
     var taskId = Guid.NewGuid(); 
     var removalStatus = hiveManager
        .GetHive(hiveId)
        .TryRemoveTask(taskId);
     ```

6. ### Clearing Hive
   - Provides functionality to clear the hive, removing all tasks and bees associated with it. This action effectively resets the hive to an empty state, ready to accept new tasks.
   - Example:
     ```csharp
     var hiveId = HiveFactory.CreateHive(2);
     var hiveManager = HiveManager.GetInstance()
     hiveManager
        .GetHive(hiveId)
        .Sudoku();
     ```

---