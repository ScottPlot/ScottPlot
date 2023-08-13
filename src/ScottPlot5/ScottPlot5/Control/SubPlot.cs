namespace ScottPlot.Control;

public class SubPlot
{
    public readonly Plot Plot;
    public PixelRect Rect;

    public SubPlot(Plot plot, PixelRect rect)
    {
        Plot = plot;
        Rect = rect;
    }
}
