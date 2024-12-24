namespace ScottPlot.SubplotPositions;

public class Fractional(FractionRect fractionRect) : ISubplotPosition
{
    public FractionRect FractionRect { get; } = fractionRect;
    public PixelRect GetRect(PixelRect figureRect)
    {
        return FractionRect.GetPixelRect(figureRect.Width, figureRect.Height);
    }
}
