using System;
using System.Collections.Generic;
using Hive.NET.Core.Models.Enums;

namespace Hive.NET.Core.Api;

internal class HiveDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<BeeWorkItemDto> WorkItems { get; set; }
    public List<BeeDto> Swarm { get; set; }
}