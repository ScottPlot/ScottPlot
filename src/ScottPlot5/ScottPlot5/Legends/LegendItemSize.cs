namespace ScottPlot.Legends;

public readonly struct LegendItemSize(PixelSize ownSize, PixelSize withChildren)
{
    public PixelSize OwnSize { get; } = ownSize;
    public PixelSize WithChildren { get; } = withChildren;
}
