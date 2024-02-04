using Hive.NET.Core.Configuration;
using Hive.NET.Core.Configuration.Notification;
using Hive.NET.Core.Manager;
using Microsoft.Extensions.Options;
using Moq;

namespace Hive.NET.Tests;

public class HiveManagerTests
{
    [Fact]
    public void CanCreateInstance()
    {
        // Arrange & Act
        var hiveManager = HiveManager.GetInstance();

        // Assert
        Assert.NotNull(hiveManager);
        Assert.IsType<HiveManager>(hiveManager);
    }

    [Fact]
    public void CanAddHive()
    {
        // Arrange
        var serviceProviderMock = new Mock<IServiceProvider>();
        var optionsMock = new Mock<IOptions<HiveSettings>>();
        var notificationMock = new Mock<INotificationProvider>();
        optionsMock.SetupGet(x => x.Value).Returns(new HiveSettings());
        serviceProviderMock.Setup(x => x.GetService(typeof(IOptions<HiveSettings>))).Returns(optionsMock.Object);
        serviceProviderMock.Setup(x => x.GetService(typeof(INotificationProvider))).Returns(notificationMock.Object);
        ServiceLocator.SetServiceProvider(serviceProviderMock.Object);
        var hiveManager = HiveManager.GetInstance();
        var hiveId = Guid.NewGuid();
        var hive = new Core.Components.Hive();

        // Act
        hiveManager.AddHive(hiveId, hive);

        // Assert
        Assert.Equal(hive, hiveManager.GetHive(hiveId));
    }


    [Fact]
    public void GetHiveThrowsExceptionForNonExistingId()
    {
        // Arrange
        var hiveManager = HiveManager.GetInstance();
        var nonExistingId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => hiveManager.GetHive(nonExistingId));
    }
}