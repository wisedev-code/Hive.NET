using Hive.NET.Core.Components;
using Hive.NET.Core.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Hive.NET.Tests;

public class BeeTests
{
    [Fact]
    public async Task DoWork_SuccessfulExecution_ReturnsTrue()
    {
        // Arrange
        var unitOfWorkMock = new BeeWorkItem(new Task(() => { Task.Delay(1000).Wait(); }));
        var serviceProviderMock = new Mock<IServiceProvider>();
        var optionsMock = new Mock<IOptions<HiveSettings>>();
        optionsMock.SetupGet(x => x.Value).Returns(new HiveSettings());
        serviceProviderMock.Setup(x => x.GetService(typeof(IOptions<HiveSettings>))).Returns(optionsMock.Object);
        ServiceLocator.SetServiceProvider(serviceProviderMock.Object);
        var bee = new Bee();

        // Act
        var result = await bee.DoWork(unitOfWorkMock, (Bee _) => Task.CompletedTask);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DoWorkExceptionThrownReturnsFalse()
    {
        // Arrange
        var unitOfWorkMock = new BeeWorkItem(
            new Task(() => throw new Exception("Simulated exception")), "example task");
        var serviceProviderMock = new Mock<IServiceProvider>();
        var optionsMock = new Mock<IOptions<HiveSettings>>();
        optionsMock.SetupGet(x => x.Value).Returns(new HiveSettings());
        serviceProviderMock.Setup(x => x.GetService(typeof(IOptions<HiveSettings>))).Returns(optionsMock.Object);
        ServiceLocator.SetServiceProvider(serviceProviderMock.Object);
        var bee = new Bee();

        // Act
        var result = await bee.DoWork(unitOfWorkMock, (Bee _) => Task.CompletedTask);

        // Assert
        Assert.False(result);
    }
}