namespace ScottPlot.Legends;

public readonly struct ItemSize(PixelSize ownSize, PixelSize withChildren)
{
    public PixelSize OwnSize { get; } = ownSize;
    public PixelSize WithChildren { get; } = withChildren;
}
