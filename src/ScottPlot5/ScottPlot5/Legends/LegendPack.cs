namespace ScottPlot.Legends;

public readonly struct LegendPack
{
    public ItemSizeAndChildren[] SizedItems { get; init; }
    public PixelRect LegendRect { get; init; }
    public PixelRect LegendShadowRect { get; init; }
    public Pixel Offset { get; init; }
}