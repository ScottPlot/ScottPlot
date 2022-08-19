namespace ScottPlot.Axis;

public interface IAxis
{
    Edge Edge { get; }
    void Render(SkiaSharp.SKSurface surface, PixelRect dataRect);
    ITickGenerator TickGenerator { get; set; }

    /// <summary>
    /// Generate ticks suitable for the given data area
    /// </summary>
    void RegenerateTicks(PixelRect dataRect);

    /// <summary>
    /// Returns only the ticks visible within the current axis limits
    /// </summary>
    /// <returns></returns>
    Tick[] GetVisibleTicks();

    /// <summary>
    /// Return the size (pixels) required to draw this axis view given the most recently generated ticks.
    /// </summary>
    float Measure();

    /// <summary>
    /// Ticks to display the next time the axis is rendered
    /// </summary>
    Tick[] Ticks { get; set; }

    /// <summary>
    /// Indicates whether or not the axis has been set intentionally.
    /// Setting is acheived by manually setting axis limits or by auto-scaling limits to fit the data.
    /// </summary>
    bool HasBeenSet { get; set; }

    /// <summary>
    /// Indicates whether the axis spans horizontally (X axis) or vertically (Y axis)
    /// </summary>
    bool IsHorizontal { get; }

    /// <summary>
    /// Returns true if the position is within the visible axis limits (inclusive on both sides)
    /// </summary>
    bool Contains(double position);

    /// <summary>
    /// Get the pixel position of a coordinate given the location and size of the data area
    /// </summary>
    float GetPixel(double position, PixelRect dataArea);

    /// <summary>
    /// Get the coordinate of a pixel position given the location and size of the data area
    /// </summary>
    double GetCoordinate(float pixel, PixelRect dataArea);

    /// <summary>
    /// Visible range
    /// </summary>
    CoordinateRange Range { get; }
}
