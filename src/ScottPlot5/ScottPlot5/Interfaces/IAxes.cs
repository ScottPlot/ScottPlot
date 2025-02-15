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

public static class IAxesExtensions
{
    public static void SetSpanX(this IAxes axes, double span)
    {
        double xMin = axes.XAxis.Range.Center - span / 2;
        double xMax = axes.XAxis.Range.Center + span / 2;
        axes.XAxis.Range.Set(xMin, xMax);
    }

    public static void SetSpanY(this IAxes axes, double span)
    {
        double yMin = axes.YAxis.Range.Center - span / 2;
        double yMax = axes.YAxis.Range.Center + span / 2;
        axes.YAxis.Range.Set(yMin, yMax);
    }

    public static PixelPath GetPixelPath(this IAxes axis, CoordinatePath path)
    {
        Pixel[] pixels = path.Points.Select(axis.GetPixel).ToArray();
        return path.Close ? PixelPath.Closed(pixels) : PixelPath.Open(pixels);
    }

    public static PixelPath[] GetPixelPaths(this IAxes axis, CoordinatePath[] paths)
    {
        return paths.Select(axis.GetPixelPath).ToArray();
    }
}
