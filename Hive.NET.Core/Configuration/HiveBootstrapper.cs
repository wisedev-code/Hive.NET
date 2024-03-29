﻿using System;
using System.Collections.Generic;
using Hive.NET.Core.Api;
using Hive.NET.Core.Configuration.Notification;
using Hive.NET.Core.Configuration.Storage;
using Hive.NET.Core.Manager;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Hive.NET.Core.Configuration;

public static class HiveBootstrapper
{
    private const string HiveSectionName = "Hive";

    public static void ConfigureHive(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IHiveManager, HiveManager>();
        services.Configure<HiveSettings>(configuration.GetSection(HiveSectionName));
        services.AddTransient<IHiveStorageProvider, EmptyStorageProvider>();
        services.AddTransient<INotificationProvider, EmptyNotificationProvider>();
    }

    public static IServiceProvider UseHive(this IServiceProvider serviceProvider)
    {
        ServiceLocator.SetServiceProvider(serviceProvider);
        var options = serviceProvider.GetRequiredService<IOptions<HiveSettings>>();
        var storageProvider = serviceProvider.GetRequiredService<IHiveStorageProvider>();
        if (!options.Value.Persistence)
        {
            return serviceProvider;
        }
        
        var hives = storageProvider.GetAllHives();
        var hiveManager = HiveManager.GetInstance();
        foreach (var hive in hives)
        {
            hiveManager.AddHive(hive!.Id, hive);    
        }

        return serviceProvider;
    }
    
    public static IApplicationBuilder UseHive(this IApplicationBuilder app)
    {
        ServiceLocator.SetServiceProvider(app.ApplicationServices);
        var options = app.ApplicationServices.GetRequiredService<IOptions<HiveSettings>>();
        var storageProvider = app.ApplicationServices.GetRequiredService<IHiveStorageProvider>();
        if (!options.Value.Persistence)
        {
            return app;
        }
        
        var hives = storageProvider.GetAllHives();
        var hiveManager = HiveManager.GetInstance();
        foreach (var hive in hives)
        {
            hiveManager.AddHive(hive!.Id, hive);    
        }

        return app;
    }
}