using System;
using Hive.NET.Core.Api;

namespace Hive.NET.Core.Components;

public class BeeError
{
    public Guid Id { get; set; }
    public string Message { get; set; }
    public string? StackTrace { get; set; }
    public string? WorkItemDescription { get; set; }
    public Guid WorkItemId { get; set; }
    public DateTime OccuredAt { get; set; }

    public BeeErrorDto MapToDto()
        => new()
        {
            OccuredAt = OccuredAt,
            StackTrace = StackTrace,
            Id = Id,
            Message = Message,
            WorkItemDescription = WorkItemDescription,
            WorkItemId = WorkItemId
        };
}