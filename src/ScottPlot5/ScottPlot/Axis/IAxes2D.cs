using System.Data;
using System.Security.Cryptography;

namespace ScottPlot.Axis;

/// <summary>
/// This interface is for an object that plottables can store
/// and use to perform coordinate/pixel conversions
/// </summary>
public interface IAxes2D
{
    /// <summary>
    /// Area in the center of the figure where plottable data will be displayed
    /// </summary>
    PixelRect DataRect { get; }

    /// <summary>
    /// This function is called immediately before rendering
    /// </summary>
    void SetDataRect(PixelRect dataRect);

    // Note: Axes (not just the translation logic) are here so ticks are accessible to plottables.
    // Note: At render time any null axes will be set to the default axes for the plot
    IXAxis XAxis { get; set; }
    IYAxis YAxis { get; set; }

    PixelRect GetPixelRect(CoordinateRect rect);

    Pixel GetPixel(Coordinates coordinates);
    float GetPixelX(double xCoordinate);
    float GetPixelY(double yCoordinate);

    Coordinates GetCoordinates(Pixel pixel);
    double GetCoordinateX(float pixel);
    double GetCoordinateY(float pixel);
}
