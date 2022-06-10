namespace ScottPlot.Axes;

public interface IAxis
{
    /// <summary>
    /// Indicates whether or not the axis has been set intentionally.
    /// Setting is acheived by manually setting axis limits or by auto-scaling limits to fit the data.
    /// </summary>
    public bool HasBeenSet { get; set; }

    /// <summary>
    /// Get the pixel position of a coordinate given the location and size of the data area
    /// </summary>
    float GetPixel(double position, PixelRect dataArea);

    /// <summary>
    /// Get the coordinate of a pixel position given the location and size of the data area
    /// </summary>
    double GetCoordinate(float pixel, PixelRect dataArea);
}
