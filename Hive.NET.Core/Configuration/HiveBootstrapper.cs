using Hive.NET.Core.Manager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.NET.Core.Configuration;

public static class HiveBootstrapper
{
    private const string HiveSectionName = "Hive";
    public static void ConfigureHive(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHiveManager, HiveManager>();
        services.Configure<HiveSettings>(configuration.GetSection(HiveSectionName));
    }

    public static void UseHive(this IServiceProvider serviceProvider)
    {
        ServiceLocator.SetServiceProvider(serviceProvider);
    }
}