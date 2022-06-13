namespace ScottPlot.AxisViews;

public interface IAxisView
{
    public Axes.IAxis Axis { get; }
    public Edge Edge { get; }
    public void Render(SkiaSharp.SKSurface surface, PixelRect dataRect);
    public ITickGenerator TickGenerator { get; set; }

    /// <summary>
    /// Generate ticks suitable for the given data area
    /// </summary>
    public void RegenerateTicks(PixelRect dataRect);

    /// <summary>
    /// Return the size (pixels) required to draw this axis view given the most recently generated ticks.
    /// </summary>
    public float Measure();
}
