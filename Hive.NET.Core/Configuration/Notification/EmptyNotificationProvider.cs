namespace Hive.NET.Core.Configuration.Notification;

public class EmptyNotificationProvider : INotificationProvider
{
    public void Notify(object message)
    {
        //base so nothing happens
    }
}