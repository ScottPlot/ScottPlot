namespace ScottPlot;

/// <summary>
/// This interface describes a pair of 1D axes.
/// It is intended to be stored inside <see cref="IPlottable"/> objects,
/// defining which axes they use and providing logic for coordinate/pixel conversions.
/// </summary>
public interface IAxes
{
    /// <summary>
    /// Describes the region in the center of the figure where plottable data will be displayed.
    /// This region is set by the renderer immediately before a Plottable's Render() method is called.
    /// </summary>
    PixelRect DataRect { get; set; }

    // Note: Axes (not just the translation logic) are here so ticks are accessible to plottables.
    // Note: At render time any null axes will be set to the default axes for the plot
    IXAxis XAxis { get; set; }
    IYAxis YAxis { get; set; }

    PixelRect GetPixelRect(CoordinateRect rect);
    PixelLine GetPixelLine(CoordinateLine rect);

    Pixel GetPixel(Coordinates coordinates);
    float GetPixelX(double xCoordinate);
    float GetPixelY(double yCoordinate);

    Coordinates GetCoordinates(Pixel pixel);
    double GetCoordinateX(float pixel);
    double GetCoordinateY(float pixel);
}
