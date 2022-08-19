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
    /// Get the pixel position of a coordinate given the location and size of the data area
    /// </summary>
    float GetPixel(double position, PixelRect dataArea);

    /// <summary>
    /// Get the coordinate of a pixel position given the location and size of the data area
    /// </summary>
    double GetCoordinate(float pixel, PixelRect dataArea);

    /// <summary>
    /// Logic for determining tick positions and formatting tick labels
    /// </summary>
    ITickGenerator TickGenerator { get; set; }

    /// <summary>
    /// Return the size (pixels) required to draw this axis view given the most recently generated ticks.
    /// </summary>
    float Measure();

    // TODO: move the label and ticks into their own interfaces each with a Render()
    /// <summary>
    /// Draw axis label and tick marks
    /// </summary>
    void Render(SkiaSharp.SKSurface surface, PixelRect dataRect);
}
