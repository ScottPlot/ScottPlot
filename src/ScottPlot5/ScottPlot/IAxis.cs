namespace ScottPlot;

// NOTE: using this abstraction now allows for non-cartesian axes in the future

/// <summary>
/// Information about the visible edges of an axis and methods to convert between coordinates and pixels
/// </summary>
public interface IAxis
{
    /// <summary>
    /// Position (coordinate space) of the lowest visible point on this axis
    /// </summary>
    public double Min { get; set; }

    /// <summary>
    /// Position (coordinate space) of the highest visible point on this axis
    /// </summary>
    public double Max { get; set; }

    /// <summary>
    /// Return the pixel location for a given coordinate
    /// </summary>
    /// <param name="position">position on this axis</param>
    /// <param name="min">pixel position of the lowest visible point on this axis</param>
    /// <param name="max">pixel position of the highest visible point on this axis</param>
    /// <returns></returns>
    public abstract float GetPixel(double position, float min, float max);
}
