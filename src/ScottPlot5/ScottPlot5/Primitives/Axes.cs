namespace ScottPlot;

/// <summary>
/// This object holds an X axis and Y axis and performs 2D coordinate/pixel conversions
/// </summary>
public class Axes : IAxes
{
    public IXAxis XAxis { get; set; } = null!;
    public IYAxis YAxis { get; set; } = null!;
    public PixelRect DataRect { get; set; }

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
