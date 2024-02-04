using System.Text.Json;
using Hive.NET.Core.Configuration.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Hive.NET.Extensions.SignalR;

public class SignalRNotificationProvider(
    IHubContext<HiveHub> hubContext,
    ILogger<SignalRNotificationProvider> logger) : INotificationProvider
{
    private readonly IHubContext<HiveHub> _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    private readonly ILogger<SignalRNotificationProvider> _logger = logger;
    public void Notify(object message)
    {
        logger.LogDebug($"Notification: {JsonSerializer.Serialize(message)}");
        _hubContext.Clients.All.SendAsync("notify", message);
        logger.LogInformation("Notification send");
    }
}