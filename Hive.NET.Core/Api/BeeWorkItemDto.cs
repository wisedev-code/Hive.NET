using System;
using Hive.NET.Core.Models.Enums;

namespace Hive.NET.Core.Api;

internal class BeeWorkItemDto
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
    public WorkItemStatus Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}