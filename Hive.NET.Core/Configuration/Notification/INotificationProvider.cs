namespace Hive.NET.Core.Configuration.Notification;

public interface INotificationProvider
{
    public void Notify(object message);
}