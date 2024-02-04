using Microsoft.AspNetCore.SignalR;

namespace Hive.NET.Extensions.SignalR;

public class HiveHub : Hub
{
    public async Task SendNotification(string message)
    {
        await Clients.All.SendAsync("Notification", message);
    }
}