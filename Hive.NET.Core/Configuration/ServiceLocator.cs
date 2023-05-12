using Microsoft.Extensions.DependencyInjection;

namespace Hive.NET.Core.Configuration;

public static class ServiceLocator
{
    private static IServiceProvider _serviceProvider;

    public static void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public static T GetService<T>()
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}