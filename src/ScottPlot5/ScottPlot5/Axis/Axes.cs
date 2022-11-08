namespace ScottPlot.Axis;

/// <summary>
/// This object performs 2D coordinate/pixel conversions based on the data area and two axes it contains.
/// </summary>
internal class Axes : IAxes
{
    public IXAxis XAxis { get; set; } = null!;
    public IYAxis YAxis { get; set; } = null!;
    public PixelRect DataRect { get; set; }

    /// <summary>
    /// <see cref="Axes"/> with null axes which will be set to defaults axes at render time.
    /// </summary>
    public static Axes Default => new();

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

    public double GetPixelDistanceX(double xDistance)
    {
        return XAxis.GetPixelDistance(xDistance, DataRect);
    }

    public double GetPixelDistanceY(double yDistance)
    {
        return YAxis.GetPixelDistance(yDistance, DataRect);
    }

    public double GetCoordinateDistanceX(double xDistance)
    {
        return XAxis.GetCoordinateDistance(xDistance, DataRect);
    }

    public double GetCoordinateDistanceY(double yDistance)
    {
        return YAxis.GetCoordinateDistance(yDistance, DataRect);
    }
}
