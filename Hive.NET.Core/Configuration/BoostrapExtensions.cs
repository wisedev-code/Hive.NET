
using Microsoft.Extensions.Logging;

namespace Hive.NET.Core.Configuration;

public static class BoostrapExtensions
{
    public static ILogger<T> BuildLogger<T>(LogLevel level)
    {
        return LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(level);
        }).CreateLogger<T>();
    }
}