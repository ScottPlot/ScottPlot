namespace ScottPlot.Axes;

public interface IAxis
{
    /* WHAT ABOUT MIN AND MAX PROPERTIES?
     * I intentionally do not have Min and Max because it's easy to get confused
     * on vertical axes whether min is the the lower number (like coordinates)
     * or a higher number (like pixels representing the bottom of the figure).
     */

    /// <summary>
    /// Indicates whether or not the axis has been set intentionally.
    /// Setting is acheived by manually setting axis limits or by auto-scaling limits to fit the data.
    /// </summary>
    public bool HasBeenSet { get; set; }

    /// <summary>
    /// Returns true if the position is within the visible axis limits (inclusive on both sides)
    /// </summary>
    public bool Contains(double position);

    /// <summary>
    /// Get the pixel position of a coordinate given the location and size of the data area
    /// </summary>
    float GetPixel(double position, PixelRect dataArea);

    /// <summary>
    /// Get the coordinate of a pixel position given the location and size of the data area
    /// </summary>
    double GetCoordinate(float pixel, PixelRect dataArea);
}
