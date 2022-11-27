using ScottPlot.LayoutSystem;

namespace ScottPlot.Axis;

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
    CoordinateRange Range { get; }

    /// <summary>
    /// Get the pixel position of a coordinate given the location and size of the data area
    /// </summary>
    float GetPixel(double position, PixelRect dataArea);

    /// <summary>
    /// Get the coordinate of a pixel position given the location and size of the data area
    /// </summary>
    double GetCoordinate(float pixel, PixelRect dataArea);

    double GetPixelDistance(double distance, PixelRect dataArea);
    double GetCoordinateDistance(double distance, PixelRect dataArea);

    /// <summary>
    /// Logic for determining tick positions and formatting tick labels
    /// </summary>
    ITickGenerator TickGenerator { get; set; }

    /// <summary>
    /// The label is the text displayed distal to the ticks
    /// </summary>
    Label Label { get; }
}
