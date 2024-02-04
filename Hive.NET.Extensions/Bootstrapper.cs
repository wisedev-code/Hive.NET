using Hive.NET.Core.Api;
using Hive.NET.Core.Configuration.Notification;
using Hive.NET.Extensions.Api;
using Hive.NET.Extensions.SignalR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.NET.Extensions;

public static class Bootstrapper
{
    public static IServiceCollection AddHiveApi(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddHostedService<HiveApi>();
    }

    public static IServiceCollection AddHiveSignalR(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSignalR();
        return serviceCollection.AddTransient<INotificationProvider, SignalRNotificationProvider>();
    }
    
    public static IApplicationBuilder MapHiveHub(this IApplicationBuilder app, string path)
    {
        return app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<HiveHub>(path);
        });
    }
}