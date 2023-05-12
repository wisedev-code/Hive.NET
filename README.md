# Hive.NET

<img align="right" height="180" width="180" style="padding: 25pt" src="https://static.vecteezy.com/system/resources/thumbnails/009/385/359/small/honeycomb-clipart-design-illustration-free-png.png"/>

<p align="left">
Welcome to Hive.NET, a powerful and efficient task management library for .NET. Just like a bustling beehive where each bee plays a vital role in maintaining the hive's productivity, BeeHive enables you to effortlessly manage and distribute tasks among a colony of hardworking "bees" (workers) within your .NET applications.
Hive.NET provides a seamless and parallel execution environment for your tasks, allowing you to achieve optimal performance and efficiency. With its intuitive API, dynamic scalability, robust concurrency control, flexible task prioritization, and comprehensive error handling, Hive.NET simplifies task management within your applications. Let the bees handle your tasks while you focus on building reliable and high-performing applications.
</p>

<to add build status and downloads>

## Features
- Task Distribution: Distribute tasks among a hive of bees for efficient and parallel execution.
- Scalability: Dynamically scale your task management capabilities by adding or removing bees from the hive.
- Concurrency Control: Ensure safe and conflict-free execution of tasks with robust concurrency control mechanisms.
- Task Prioritization: Assign priority levels to tasks to optimize performance and responsiveness.
- Error Handling: Handle exceptions and ensure fault tolerance for reliable task execution.

## Installation
To make use of Hive.NET library and its most important features you need to install nuget package, you can do this with *dotnet* command like this:
> dotnet add package Hive.NET

or via nuget package manager console:

> Install-Package Hive.NET

To check newest updates and additional information please check direct link:
https://www.nuget.org/packages/Hive.NET

## Getting started

To start using Hive.NET in your .NET projects, follow these simple steps:

Register Dependencies: Begin by registering the necessary dependencies for Hive.NET in your application. This step ensures that the required services and configurations are available for Hive.NET to function properly. Below is an example using the .NET Minimal API:

```
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((_, services) =>
    {
        services.ConfigureHive(configuration);
    }).Build();

host.Services.UseHive();
```

In the above example, we utilize the Host.CreateDefaultBuilder() method to create a default host builder. Within the ConfigureServices method, we configure the hive by calling services.ConfigureHive(configuration), where configuration represents your specific configuration settings. Finally, we use the UseHive() method to enable Hive.NET within the host's services, making it available for usage throughout your application.

In actual code at beggining you will have to create hive.
```
var hiveId = HiveFactory.CreateHive(2); //you have to provide swarn size
var hive = _manager.GetHive(hiveId);
```
May be good idea to persist hiveId somewhere as you also may want to use it later on. Once we have our hive initialized, you can start adding task that you want to be performed by your bees.

```
hive.AddTask(
    new BeeWorkItem(
        new Task(() =>
        {
            // Perform some work here
        }),
        () => System.Console.WriteLine("Finished! :) "),
        () => System.Console.WriteLine("Failed! :( ")
    )
);
```

In the above example, the AddTask method is used to add a BeeWorkItem to the hive. The BeeWorkItem represents a unit of work to be executed by a bee. It takes three parameters:

- A Task object that encapsulates the actual work to be performed. In the provided example, the work is performed inside the lambda expression.
- A success callback that is invoked when the task is successfully completed. Here, System.Console.WriteLine("Finished! :)") is used as an example success callback, which can be replaced with your own desired logic.
- A failure callback that is invoked when the task encounters an error or fails to complete. In the given example, System.Console.WriteLine("Failed! :(") is used as an example failure callback, which can be customized according to your specific error handling requirements.

By using the BeeWorkItem approach, you can add tasks with their corresponding success and failure callbacks, providing more control and flexibility over the execution flow.

With this approach now when we start our demo app, with swarn size 2 and simple delaying tasks, thats how result will look like (we provided 3 tasks)

![image](https://github.com/wisedev-code/Hive.NET/assets/111281468/7ac1f705-6b82-49f0-b789-2897f80f0151)

## Docs

We are not there yet :) 

<img  height="200" width="200" style="padding: 25pt" src="https://cdn-icons-png.flaticon.com/512/5229/5229377.png"/>

## Why the name Hive?
    
<img align="right" height="210" width="210" style="padding: 25pt" src="https://pngimg.com/uploads/bee/bee_PNG74646.png"/>
    
<p align="left">    
The journey of naming our animal reference libraries began with "Raven.NET" and now continues with "Hive.NET". While "Raven.NET" embodies the observer-like qualities of a raven, "Hive.NET" draws inspiration from the hardworking nature of bees. In developing Hive.NET, we aimed to create a library that embraces the diligent and collaborative nature of bees within a hive. Bees work together, observing their environment, coordinating their efforts, and executing tasks harmoniously. Similarly, Hive.NET enables you to distribute and manage tasks among a colony of bees (workers) in your application, ensuring efficient and parallel execution.
The choice of "Hive.NET" reflects the library's emphasis on observation and coordination. Bees are known for their ability to observe their surroundings keenly, communicate with one another, and work together towards a common goal. Hive.NET provides a framework that allows you to observe and coordinate the execution of tasks, optimizing performance and responsiveness within your application.
</p>
