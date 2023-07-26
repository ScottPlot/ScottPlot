using SkiaSharp;

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
    public Coordinates Center => new(XCenter, YCenter);
    public double Width => XMax - XMin;
    public double Height => YMax - YMin;
    public double Area => Width * Height;
    public bool HasArea => (Area != 0 && !double.IsNaN(Area) && !double.IsInfinity(Area));
    public CoordinateRange XRange => new(XMin, XMax);
    public CoordinateRange YRange => new(YMin, YMax);

    public CoordinateRect(CoordinateRange xRange, CoordinateRange yRange)
    {
        XMin = xRange.Min;
        XMax = xRange.Max;
        YMin = yRange.Min;
        YMax = yRange.Max;
    }

    public CoordinateRect(Coordinates pt1, Coordinates pt2)
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

    public CoordinateRect(Coordinates point, CoordinateSize size)
    {
        Coordinates pt2 = new(point.X + size.Width, point.Y + size.Height);
        XMin = Math.Min(point.X, pt2.X);
        XMax = Math.Max(point.X, pt2.X);
        YMin = Math.Min(point.Y, pt2.Y);
        YMax = Math.Max(point.Y, pt2.Y);
    }

    public static CoordinateRect Empty => new(double.NaN, double.NaN, double.NaN, double.NaN);

    public CoordinateRect WithTranslation(Coordinates p) => new(XMin + p.X, XMax + p.X, YMin + p.Y, YMax + p.Y);

    public override string ToString()
    {
        return $"PixelRect: XMin={XMin} XMax={XMax} YMin={YMin} YMax={YMax}";
    }
}
