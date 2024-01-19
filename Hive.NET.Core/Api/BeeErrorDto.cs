using System;

namespace Hive.NET.Core.Api;

public class BeeErrorDto
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public string WorkItemDescription { get; set; }
    public Guid WorkItemId { get; set; }
    public DateTime OccuredAt { get; set; }
}