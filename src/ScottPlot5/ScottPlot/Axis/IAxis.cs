namespace ScottPlot.Axis;

/// <summary>
/// This interface describes a 1D axis (horizontal or vertical).
/// Responsibilities include: min/max management, unit/pixel conversion, 
/// tick generation (and rendering), axis label rendering, 
/// and self-measurement for layout purposes.
/// </summary>
public interface IAxis
{
    /// <summary>
    /// Describes which edge this 1D axis represents
    /// </summary>
    Edge Edge { get; }

    // TODO: move the label and ticks into their own interfaces each with a Render()
    /// <summary>
    /// Draw axis label and tick marks
    /// </summary>
    void Render(SkiaSharp.SKSurface surface, PixelRect dataRect);

    /// <summary>
    /// Return the size (pixels) required to draw this axis view given the most recently generated ticks.
    /// </summary>
    float Measure();






    /// <summary>
    /// Logic for determining positions and labels for ticks nd grids
    /// </summary>
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
    /// Ticks to display the next time the axis is rendered
    /// </summary>
    Tick[] Ticks { get; set; }










    /// <summary>
    /// Min/Max range currently displayed by this axis
    /// </summary>
    CoordinateRange Range { get; }

    /// <summary>
    /// Indicates whether or not the axis has been set intentionally.
    /// Setting is achieved by manually setting axis limits or by auto-scaling limits to fit the data.
    /// </summary>
    bool HasBeenSet { get; set; }

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
}
