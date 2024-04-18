namespace ScottPlot.Legends;

public readonly struct ItemSizeAndChildren(LegendItem item, ItemSize size, ItemSizeAndChildren[] children)
{
    public LegendItem Item { get; } = item;
    public ItemSize Size { get; } = size;
    public ItemSizeAndChildren[] Children { get; } = children;
}
