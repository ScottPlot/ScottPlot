namespace ScottPlot;

/// <summary>
/// This interface describes a 1D axis (horizontal or vertical).
/// Responsibilities include: min/max management, unit/pixel conversion, 
/// tick generation (and rendering), axis label rendering, 
/// and self-measurement for layout purposes.
/// </summary>
public interface IAxis : IPanel
{
    /// <summary>
    /// Min/Max range currently displayed by this axis
    /// </summary>
    CoordinateRangeMutable Range { get; } // TODO: don't expose this

    double Min { get; set; }
    double Max { get; set; }

    /// <summary>
    /// Get the pixel position of a coordinate given the location and size of the data area
    /// </summary>
    float GetPixel(double position, PixelRect dataArea);

    /// <summary>
    /// Get the coordinate of a pixel position given the location and size of the data area
    /// </summary>
    double GetCoordinate(float pixel, PixelRect dataArea);

    /// <summary>
    /// Given a distance in coordinate space, converts to pixel space
    /// </summary>
    /// <param name="coordinateDistance">A distance in coordinate units</param>
    /// <param name="dataArea">The rectangle onto which the coordinates are mapped</param>
    /// <returns>The same distance in pixel units</returns>
    double GetPixelDistance(double coordinateDistance, PixelRect dataArea);

    /// <summary>
    /// Given a distance in pixel space, converts to coordinate space
    /// </summary>
    /// <param name="pixelDistance">A distance in pixel units</param>
    /// <param name="dataArea">The rectangle onto which the coordinates are mapped</param>
    /// <returns>The same distance in coordinate units</returns>
    double GetCoordinateDistance(float pixelDistance, PixelRect dataArea);

    /// <summary>
    /// Logic for determining tick positions and formatting tick labels
    /// </summary>
    ITickGenerator TickGenerator { get; set; } // TODO: never call TickGenerator.Generate() externally

    /// <summary>
    /// Replace the <see cref="TickGenerator"/> with a <see cref="NumericManual"/> pre-loaded with the given ticks.
    /// </summary>
    public void SetTicks(double[] xs, string[] labels);

    /// <summary>
    /// Use the <see cref="TickLabelStyle"/> to generate ticks with ideal spacing.
    /// </summary>
    public void RegenerateTicks(PixelLength size);

    /// <summary>
    /// The label is the text displayed distal to the ticks
    /// </summary>
    LabelStyle Label { get; }

    TickMarkStyle MajorTickStyle { get; set; }

    TickMarkStyle MinorTickStyle { get; set; }

    LabelStyle TickLabelStyle { get; set; }

    LineStyle FrameLineStyle { get; }
}

public static class IAxisExtensions
{
    public static CoordinateRange GetRange(this IAxis axis) => new(axis.Min, axis.Max);
    public static bool IsInverted(this IAxis axis) => axis.Min > axis.Max;
    public static void RemoveTickGenerator(this IAxis axis) => axis.TickGenerator = new TickGenerators.EmptyTickGenerator();

    /// <summary>
    /// Set the axis size to zero and disable tick generation.
    /// This method allows the data area to rest against the edge of the figure
    /// (e.g., when collapsing space between stacked plots in a multiplot)
    /// </summary>
    public static void Collapse(this IAxis axis)
    {
        axis.MaximumSize = 0;
        axis.RemoveTickGenerator();
    }
}
