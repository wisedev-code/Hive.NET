using System;
using System.Collections.Concurrent;
using System.Linq;
using Hive.NET.Core.Api;
using Hive.NET.Core.Components;
using Hive.NET.Core.Models.Enums;

namespace Hive.NET.Core.Extensions;

public static class Extensions
{
    internal static Components.Hive MapFromDetails(this HiveDetailsDto detailsDto) =>
        new()
        {
            Id = detailsDto.Id,
            _name = detailsDto.Name,
            Swarm = detailsDto.Swarm.Select(x => new Bee()
            {
                IsWorking = x.IsWorking,
                Id = x.Id
            }).ToList(),
            Items = detailsDto.WorkItems.ToList().Select(workItem =>
                new BeeWorkItem(id: workItem.Id, description: workItem.Description)).ToList(),
            Statuses = new ConcurrentDictionary<Guid, (WorkItemStatus Status, DateTime UpdatedAt)>(
                detailsDto.WorkItems.ToDictionary(x => x.Id, x => (x.Status, x.UpdatedAt) ))
        };
}