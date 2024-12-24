namespace ScottPlot.SubplotPositions;

public class Full : ISubplotPosition
{
    public PixelRect GetRect(PixelRect figureRect)
    {
        return figureRect;
    }
}
