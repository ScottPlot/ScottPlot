namespace ScottPlot;

/// <summary>
/// Describes a rectangle in 2D coordinate space.
/// </summary>
public struct CoordinateRect
{
    public double XMin { get; set; }
    public double YMin { get; set; }
    public double XMax { get; set; }
    public double YMax { get; set; }

    public double XCenter => (XMax + XMin) / 2;
    public double YCenter => (YMax + YMin) / 2;
    public double Width => XMax - XMin;
    public double Height => YMax - YMin;
    public bool HasArea => Width * Height != 0;

    public CoordinateRect(Coordinate pt1, Coordinate pt2)
    {
        XMin = Math.Min(pt1.X, pt2.X);
        XMax = Math.Max(pt1.X, pt2.X);
        YMin = Math.Min(pt1.Y, pt2.Y);
        YMax = Math.Max(pt1.Y, pt2.Y);
    }

    public CoordinateRect(double xMin, double xMax, double yMin, double yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }

    public override string ToString()
    {
        return $"PixelRect: XMin={XMin} XMax={XMax} YMin={YMin} YMax={YMax}";
    }

    public CoordinateRect WithPan(double deltaX, double deltaY)
    {
        return new CoordinateRect(XMin + deltaX, XMax + deltaX, YMin + deltaY, YMax + deltaY);
    }

    public CoordinateRect WithZoom(double fracX, double fracY)
    {
        return WithZoom(fracX, fracY, XCenter, YCenter);
    }

    public CoordinateRect WithZoom(double fracX, double fracY, double zoomToX, double zoomToY)
    {
        double spanLeftX = zoomToX - XMin;
        double spanRightX = XMax - zoomToX;
        double newMinX = zoomToX - spanLeftX / fracX;
        double newMaxX = zoomToX + spanRightX / fracX;

        double spanLeftY = zoomToY - YMin;
        double spanRightY = YMax - zoomToY;
        double newMinY = zoomToY - spanLeftY / fracY;
        double newMaxY = zoomToY + spanRightY / fracY;

        return new CoordinateRect(newMinX, newMaxX, newMinY, newMaxY);
    }
}