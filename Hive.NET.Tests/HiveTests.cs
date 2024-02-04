using Hive.NET.Core.Components;
using Hive.NET.Core.Configuration;
using Hive.NET.Core.Configuration.Notification;
using Hive.NET.Core.Configuration.Storage;
using Hive.NET.Core.Models.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Hive.NET.Tests;

public class HiveTests
{
    [Fact]
    public void AddTask_ReturnsValidTaskGuid()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        var task = new BeeWorkItem(new Task(() => Task.Delay(100).Wait()));

        // Act
        var taskId = hive.AddTask(task);

        // Assert
        Assert.NotEqual(Guid.Empty, taskId);
        Assert.True(hive.Statuses.ContainsKey(taskId));
        Assert.Equal(WorkItemStatus.Waiting, hive.Statuses[taskId].Status);
    }

    [Fact]
    public void GetWorkItemStatus_ExistingTask_ReturnsCorrectStatus()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        var taskId = hive.AddTask(new BeeWorkItem(new Task(() => Task.Delay(100).Wait())));

        // Act
        var status = hive.GetWorkItemStatus(taskId);

        // Assert
        Assert.Equal(WorkItemStatus.Waiting, status.Status);
    }

    [Fact]
    public void GetWorkItemStatus_NonexistentTask_ReturnsNotExistStatus()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        var nonExistentTaskId = Guid.NewGuid();

        // Act
        var status = hive.GetWorkItemStatus(nonExistentTaskId);

        // Assert
        Assert.Equal(WorkItemStatus.NotExist, status.Status);
    }

    [Fact]
    public void WorkItemStatusTryRemoveTask_ExistingTask_ReturnsRemovedStatus()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        var taskId = hive.AddTask(new BeeWorkItem(new Task(() => Task.Delay(100))));

        // Act
        var status = hive.TryRemoveTask(taskId);

        // Assert
        Assert.Equal(WorkItemStatus.Removed, status);
    }

    [Fact]
    public void WorkItemStatusTryRemoveTask_NonexistentTask_ReturnsNotExistStatus()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        var nonExistentTaskId = Guid.NewGuid();

        // Act
        var status = hive.TryRemoveTask(nonExistentTaskId);

        // Assert
        Assert.Equal(WorkItemStatus.NotExist, status);
    }
    
       [Fact]
    public void MapToDto_ReturnsValidHiveDto()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        
        // Act
        var hiveDto = hive.MapToDto();

        // Assert
        Assert.Equal(hive.Id, hiveDto.Id);
        Assert.Equal(hive._name, hiveDto.Name);
        Assert.Equal(hive.Swarm.Count, hiveDto.Bees.Count);
        foreach (var beeDto in hiveDto.Bees)
        {
            var correspondingBee = hive.Swarm.FirstOrDefault(bee => bee.Id == beeDto.Id);
            Assert.NotNull(correspondingBee);
            Assert.Equal(correspondingBee.IsWorking, beeDto.IsWorking);
        }
    }

    [Fact]
    public void MapToDetailsDto_ReturnsValidHiveDetailsDto()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        
        // Act
        var hiveDetailsDto = hive.MapToDetailsDto();

        // Assert
        Assert.Equal(hive.Id, hiveDetailsDto.Id);
        Assert.Equal(hive._name, hiveDetailsDto.Name);
        Assert.Equal(hive.Swarm.Count, hiveDetailsDto.Swarm.Count);
        Assert.Equal(hive.Items.Count, hiveDetailsDto.WorkItems.Count);

        foreach (var beeDto in hiveDetailsDto.Swarm)
        {
            var correspondingBee = hive.Swarm.FirstOrDefault(bee => bee.Id == beeDto.Id);
            Assert.NotNull(correspondingBee);
            Assert.Equal(correspondingBee.IsWorking, beeDto.IsWorking);
        }

        foreach (var workItemDto in hiveDetailsDto.WorkItems)
        {
            Assert.True((bool)hive.Statuses.ContainsKey(workItemDto.Id));
            Assert.Equal(hive.Statuses[workItemDto.Id].Status, workItemDto.Status);
            Assert.Equal(hive.Statuses[workItemDto.Id].UpdatedAt, workItemDto.UpdatedAt);
            Assert.NotNull(hive.Items.FirstOrDefault(item => item.Id == workItemDto.Id));
        }
    }

    [Fact]
    public void MapToErrorsDto_ReturnsValidBeeErrorDtoList()
    {
        // Arrange
        ArrangeFixture();
        var hive = new Core.Components.Hive(3);
        var bee = new Bee();
        bee.RegisteredErrors.Add(new BeeError
        {
            Id = Guid.NewGuid(),
            Message = "Error 1",
            StackTrace = "Stack Trace 1",
            WorkItemDescription = "Description 1",
            WorkItemId = Guid.NewGuid(),
            OccuredAt = DateTime.UtcNow
        });

        hive.Swarm.Add(bee);

        // Act
        var errorsDto = hive.MapToErrorsDto();

        // Assert
        Assert.Single(errorsDto);
        Assert.Equal(bee.RegisteredErrors[0].Id, errorsDto[0].Id);
        Assert.Equal(bee.RegisteredErrors[0].Message, errorsDto[0].Message);
        Assert.Equal(bee.RegisteredErrors[0].StackTrace, errorsDto[0].StackTrace);
        Assert.Equal(bee.RegisteredErrors[0].WorkItemDescription, errorsDto[0].WorkItemDescription);
        Assert.Equal(bee.RegisteredErrors[0].WorkItemId, errorsDto[0].WorkItemId);
        Assert.Equal(bee.RegisteredErrors[0].OccuredAt, errorsDto[0].OccuredAt);
    }

    private void ArrangeFixture()
    {
        var serviceProviderMock = new Mock<IServiceProvider>();
        var optionsMock = new Mock<IOptions<HiveSettings>>();
        var hiveStorageMock = new Mock<IHiveStorageProvider>();
        var notificationProvider = new Mock<INotificationProvider>();
        optionsMock.SetupGet(x => x.Value).Returns(new HiveSettings());
        serviceProviderMock.Setup(x => x.GetService(typeof(INotificationProvider)))
            .Returns(notificationProvider.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(IOptions<HiveSettings>))).Returns(optionsMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(IHiveStorageProvider))).Returns(hiveStorageMock.Object);
        ServiceLocator.SetServiceProvider(serviceProviderMock.Object);
    }
}