using System;
using System.Collections.Generic;
using System.Linq;

namespace Hive.NET.Core.Components.BeeWorkItems;

public class BeeWorkItemsSequence
{
    public Guid Id;
    public string Description;
    private List<BeeWorkItem> items = [];

    public BeeWorkItemsSequence(string description)
    {
        Id = Guid.NewGuid();
        Description = description;
    }
    
    public BeeWorkItemsSequence(string description, List<BeeWorkItem> workItems)
    {
        Id = Guid.NewGuid();
        Description = description;
        var count = 1;
        foreach (var item in workItems)
        {
            item.Description += $" ({Id}-{count}) ";
            items.Add(item);
            count++;
        }
    }

    public void AddWorkItem(BeeWorkItem item)
    {
        item.Description += $" ({Id}-{items.Count+1}) ";
        items.Add(item);
    }

    public void RemoveItem(BeeWorkItem item)
    {
        items.Remove(item);
    }

    public void Clear()
    {
        items.Clear();
    }

    public IEnumerable<BeeWorkItem> Find(Func<BeeWorkItem, int, bool> predicate)
    {
        return items.Where(predicate);
    }
    
    public IEnumerable<BeeWorkItem> All()
    {
        return items;
    }
}