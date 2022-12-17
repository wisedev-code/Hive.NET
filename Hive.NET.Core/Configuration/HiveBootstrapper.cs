using Hive.NET.Core.Manager;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.NET.Core.Configuration;

public static class HiveBootstrapper
{
    public static void ConfigureHive(this IServiceCollection services)
    {
        services.AddSingleton<IHiveManager, HiveManager>();
    }
}