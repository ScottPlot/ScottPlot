namespace ScottPlot.Legends;

public readonly struct LegendPack
{
    required public LegendItem[] LegendItems { get; init; }
    required public PixelRect[] LegendItemRects { get; init; }
    required public PixelRect LegendRect { get; init; }
}