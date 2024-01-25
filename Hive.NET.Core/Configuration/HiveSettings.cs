using Microsoft.Extensions.Logging;

namespace Hive.NET.Core.Configuration;

public class HiveSettings
{
    public LogLevel LogLevel { get; set; } = LogLevel.Warning;
    public bool Persistence { get; set; } = false;
}