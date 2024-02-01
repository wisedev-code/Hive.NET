using Hive.NET.Core.Components;

namespace Hive.NET.Tests;

public class BeeWorkItemSequenceTests
{
    [Fact]
    public void AddTask_ShouldCreateSequenceWithCorrectDescriptions()
    {
        // Arrange
        BeeWorkItemsSequence taskSequence = new BeeWorkItemsSequence("Test Sequence");
        BeeWorkItem task1 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 1")));
        BeeWorkItem task2 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 2")));
        BeeWorkItem task3 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 3")));

        // Act
        taskSequence.AddWorkItem(task1);
        taskSequence.AddWorkItem(task2);
        taskSequence.AddWorkItem(task3);

        // Assert
        Assert.Equal(3, taskSequence.All().Count());

        var descriptions = taskSequence.All().Select(item => item.Description).ToList();
        Assert.Contains($" ({taskSequence.Id}-1) ", descriptions);
        Assert.Contains($" ({taskSequence.Id}-2) ", descriptions);
        Assert.Contains($" ({taskSequence.Id}-3) ", descriptions);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveItemFromSequence()
    {
        // Arrange
        BeeWorkItemsSequence taskSequence = new BeeWorkItemsSequence("Test Sequence");
        BeeWorkItem task1 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 1")));

        // Act
        taskSequence.AddWorkItem(task1);
        taskSequence.RemoveItem(task1);

        // Assert
        Assert.Empty(taskSequence.All());
    }

    [Fact]
    public void Clear_ShouldRemoveAllItemsFromSequence()
    {
        // Arrange
        BeeWorkItemsSequence taskSequence = new BeeWorkItemsSequence("Test Sequence");
        BeeWorkItem task1 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 1")));
        BeeWorkItem task2 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 2")));

        // Act
        taskSequence.AddWorkItem(task1);
        taskSequence.AddWorkItem(task2);
        taskSequence.Clear();

        // Assert
        Assert.Empty(taskSequence.All());
    }

    [Fact]
    public void Find_ShouldReturnMatchingItems()
    {
        // Arrange
        BeeWorkItemsSequence taskSequence = new BeeWorkItemsSequence("Test Sequence");
        BeeWorkItem task1 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 1")));
        BeeWorkItem task2 = new BeeWorkItem(new Task(() => Console.WriteLine("Task 2")));

        // Act
        taskSequence.AddWorkItem(task1);
        taskSequence.AddWorkItem(task2);

        // Assert
        var foundItems = taskSequence.Find((item, index) => index == 1);
        Assert.Single(foundItems);
        Assert.Contains(task2, foundItems);
    }
}