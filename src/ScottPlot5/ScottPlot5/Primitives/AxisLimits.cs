namespace ScottPlot;

/// <summary>
/// This object represents the rectangular visible area on a 2D coordinate system.
/// It simply stores a <see cref="CoordinateRect"/> but has axis-related methods to act upon it.
/// </summary>
public readonly struct AxisLimits
{
    public double XMin { get; }
    public double XMax { get; }
    public double YMin { get; }
    public double YMax { get; }

    public double Left => XMin;
    public double Right => XMax;
    public double Bottom => YMin;
    public double Top => YMax;
    public double Width => XMax - XMin;
    public double Height => YMax - YMin;

    // TODO: make sure callers aren't using this when they dont have to
    public CoordinateRect Rect => new(XMin, XMax, YMin, YMax);

    public static CoordinateRect Default { get; } = new(-10, 10, -10, 10);

    public AxisLimits(CoordinateRect rect)
    {
        XMin = rect.XMin;
        XMax = rect.XMax;
        YMin = rect.YMin;
        YMax = rect.YMax;
    }

    public AxisLimits(double xMin, double xMax, double yMin, double yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }

    public AxisLimits(CoordinateRange xRange, CoordinateRange yRange)
    {
        XMin = xRange.Min;
        XMax = xRange.Max;
        YMin = yRange.Min;
        YMax = yRange.Max;
    }

    public override string ToString()
    {
        return $"AxisLimits: X=[{Rect.XMin}, {Rect.XMax}], Y=[{Rect.YMin}, {Rect.YMax}]";
    }

    public static AxisLimits NoLimits => new(double.NaN, double.NaN, double.NaN, double.NaN);

    /// <summary>
    /// Return a new <see cref="AxisLimits"/> expanded to include the given <paramref name="x"/> and <paramref name="y"/>.
    /// </summary>
    public AxisLimits Expanded(double x, double y)
    {
        double xMin2 = !double.IsNaN(XMin) ? Math.Min(XMin, x) : x;
        double xMax2 = !double.IsNaN(XMax) ? Math.Max(XMax, x) : x;
        double yMin2 = !double.IsNaN(YMin) ? Math.Min(YMin, y) : y;
        double yMax2 = !double.IsNaN(YMax) ? Math.Max(YMax, y) : y;
        return new AxisLimits(xMin2, xMax2, yMin2, yMax2);
    }

    /// <summary>
    /// Return a new <see cref="AxisLimits"/> expanded to include the given <paramref name="coordinates"/>.
    /// </summary>
    public AxisLimits Expanded(Coordinates coordinates)
    {
        return Expanded(coordinates.X, coordinates.Y);
    }

    /// <summary>
    /// Return a new <see cref="AxisLimits"/> expanded to include all corners of the given <paramref name="rect"/>.
    /// </summary>
    public AxisLimits Expanded(CoordinateRect rect)
    {
        return Expanded(rect.TopLeft).Expanded(rect.BottomRight);
    }

    public CoordinateRect WithPan(double deltaX, double deltaY)
    {
        return new CoordinateRect(Rect.XMin + deltaX, Rect.XMax + deltaX, Rect.YMin + deltaY, Rect.YMax + deltaY);
    }

    public CoordinateRect WithZoom(double fracX, double fracY)
    {
        return WithZoom(fracX, fracY, Rect.XCenter, Rect.YCenter);
    }

    public CoordinateRect WithZoom(double fracX, double fracY, double zoomToX, double zoomToY)
    {
        CoordinateRange xRange = new(Rect.XMin, Rect.XMax);
        xRange.ZoomFrac(fracX, zoomToX);

        CoordinateRange yRange = new(Rect.YMin, Rect.YMax);
        yRange.ZoomFrac(fracY, zoomToY);

        return new(xRange, yRange);
    }
}
