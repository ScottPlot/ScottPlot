namespace ScottPlot.Legends;

public readonly struct SizedLegendItem(LegendItem item, LegendItemSize size, SizedLegendItem[] children)
{
    public LegendItem Item { get; } = item;
    public LegendItemSize Size { get; } = size;
    public SizedLegendItem[] Children { get; } = children;
}
