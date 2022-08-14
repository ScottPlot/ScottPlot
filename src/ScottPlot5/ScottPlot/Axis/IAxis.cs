namespace ScottPlot.Axis;

public interface IAxis
{
    public AxisTranslation.IAxisTranslator Translator { get; }
    public Edge Edge { get; }
    public void Render(SkiaSharp.SKSurface surface, PixelRect dataRect);
    public ITickGenerator TickGenerator { get; set; }

    /// <summary>
    /// Generate ticks suitable for the given data area
    /// </summary>
    public void RegenerateTicks(PixelRect dataRect);

    /// <summary>
    /// Returns only the ticks visible within the current axis limits
    /// </summary>
    /// <returns></returns>
    public Tick[] GetVisibleTicks();

    /// <summary>
    /// Return the size (pixels) required to draw this axis view given the most recently generated ticks.
    /// </summary>
    public float Measure();

    /// <summary>
    /// Ticks to display the next time the axis is rendered
    /// </summary>
    public Tick[] Ticks { get; set; }
}
