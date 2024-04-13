namespace ScottPlot;

/// <summary>
/// This object holds an X axis and Y axis and performs 2D coordinate/pixel conversions
/// </summary>
public class Axes : IAxes
{
    // TODO: these should probably be readonly and passed into the constructor
    public IXAxis XAxis { get; set; } = null!;
    public IYAxis YAxis { get; set; } = null!;
    public PixelRect DataRect { get; set; }

    public static Axes Default => new();

    public Axes()
    {
    }

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

    public PixelLine GetPixelLine(CoordinateLine line)
    {
        Pixel pt1 = GetPixel(line.Start);
        Pixel pt2 = GetPixel(line.End);
        return new PixelLine(pt1, pt2);
    }

    public float GetPixelX(double xCoordinate) => XAxis.GetPixel(xCoordinate, DataRect);

    public float GetPixelY(double yCoordinate) => YAxis.GetPixel(yCoordinate, DataRect);

    public PixelRect GetPixelRect(CoordinateRect rect)
    {
        return new PixelRect(
            left: GetPixelX(rect.Left),
            right: GetPixelX(rect.Right),
            bottom: GetPixelY(rect.Bottom),
            top: GetPixelY(rect.Top));
    }
}
