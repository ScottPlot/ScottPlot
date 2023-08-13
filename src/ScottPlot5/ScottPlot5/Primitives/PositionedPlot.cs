namespace ScottPlot;

/// <summary>
/// This object pairs a <see cref="Plot"/> with a <see cref="PixelRect"/> 
/// that describes where on the canvas it should be rendered
/// </summary>
public readonly struct PositionedPlot
{
    public readonly Plot Plot;
    public readonly PixelRect Rect;

    public PositionedPlot(Plot plot, PixelRect rect)
    {
        Plot = plot;
        Rect = rect;
    }

    public void Render(SKCanvas canvas)
    {
        Plot.Render(canvas, Rect);
    }
}
