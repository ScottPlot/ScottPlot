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

    public double Width => XMax - XMin;
    public double Height => YMax - YMin;
    public bool HasArea => Width * Height != 0;

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
}