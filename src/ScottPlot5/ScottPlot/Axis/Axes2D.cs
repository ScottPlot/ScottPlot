namespace ScottPlot.Axis;

/// <summary>
/// This object performs 2D coordinate/pixel conversions based on the data area and two axes it contains.
/// </summary>
internal class Axes2D : IAxes2D
{
    public IXAxis XAxis { get; set; }
    public IYAxis YAxis { get; set; }
    public AxisLimits View => new(XAxis.Range, YAxis.Range);
    public PixelRect DataRect { get; private set; }

    /// <summary>
    /// Create without initial axes (default axes will be automatically assigned at render time)
    /// </summary>
    public Axes2D()
    {
        XAxis = null!;
        YAxis = null!;
    }

    /// <summary>
    /// Create using pre-defined axes
    /// </summary>
    public Axes2D(IXAxis xAxis, IYAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }

    /// <summary>
    /// This will be called to update the data area dimensions immediately before rendering
    /// </summary>
    public void SetDataRect(PixelRect dataRect)
    {
        DataRect = dataRect;
    }

    /// <summary>
    /// <see cref="Axes2D"/> with null axes which will be set to defaults axes at render time.
    /// </summary>
    public static Axes2D Default => new();

    public Coordinates GetCoordinates(Pixel pixel)
    {
        double x = XAxis.GetCoordinate(pixel.X, DataRect);
        double y = YAxis.GetCoordinate(pixel.Y, DataRect);
        return new Coordinates(x, y);
    }

    public double GetCoordinateX(float pixel) => XAxis.GetCoordinate(pixel, DataRect);

    public double GetCoordinateY(float pixel) => YAxis.GetCoordinate(pixel, DataRect);

    public Pixel GetPixel(Coordinates coordinates)
    {
        float x = XAxis.GetPixel(coordinates.X, DataRect);
        float y = YAxis.GetPixel(coordinates.Y, DataRect);
        return new Pixel(x, y);
    }

    public float GetPixelX(double xCoordinate) => XAxis.GetPixel(xCoordinate, DataRect);

    public float GetPixelY(double yCoordinate) => YAxis.GetPixel(yCoordinate, DataRect);

    public PixelRect GetPixelRect(CoordinateRect rect)
    {
        return new PixelRect(
            left: GetPixelX(rect.XMin),
            right: GetPixelX(rect.XMax),
            bottom: GetPixelY(rect.YMin),
            top: GetPixelY(rect.YMax));
    }
}
