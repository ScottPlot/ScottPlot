namespace ScottPlot.SubplotPositions;

public class Absolute(PixelRect pixelRect) : ISubplotPosition
{
    public PixelRect PixelRect { get; } = pixelRect;
    public PixelRect GetRect(PixelRect figureRect)
    {
        return PixelRect;
    }
}
