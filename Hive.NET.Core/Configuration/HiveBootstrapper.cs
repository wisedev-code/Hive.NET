using System;
using Hive.NET.Core.Api;
using Hive.NET.Core.Configuration.Storage;
using Hive.NET.Core.Manager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hive.NET.Core.Configuration;

public static class HiveBootstrapper
{
    private const string HiveSectionName = "Hive";

    public static void ConfigureHive(this IServiceCollection services, IConfiguration configuration,
        bool persistence = false)
    {
        services.AddSingleton<IHiveManager>()
        services.Configure<HiveSettings>(configuration.GetSection(HiveSectionName));
        services.AddTransient<IHiveStorageProvider, EmptyStorageProvider>();
        services.AddHostedService<HiveApi>();
    }

    public static IServiceProvider UseHive(this IServiceProvider serviceProvider)
    {
        ServiceLocator.SetServiceProvider(serviceProvider);
        return serviceProvider;
    }
}