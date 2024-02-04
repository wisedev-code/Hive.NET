using Hive.NET.Core.Components;
using Hive.NET.Core.Components.BeeWorkItems;

namespace Hive.NET.Extensions.Components;

public static class BeeWorkItemsSequenceExtensions
{
    public static BeeWorkItemsSequence AddWorkItem(this BeeWorkItemsSequence sequence, BeeWorkItem item)
    {
        sequence.AddWorkItem(item);
        return sequence;
    }

    public static BeeWorkItemsSequence AddWorkItems(this BeeWorkItemsSequence sequence, IEnumerable<BeeWorkItem> items)
    {
        foreach (var item in items)
        {
            sequence.AddWorkItem(item);
        }
        return sequence;
    }

    public static BeeWorkItemsSequence RemoveWorkItem(this BeeWorkItemsSequence sequence, BeeWorkItem item)
    {
        sequence.RemoveItem(item);
        return sequence;
    }
}