namespace ScottPlot;

public readonly struct AxisLimits3d
{
    public readonly double XMin;
    public readonly double XMax;

    public readonly double YMin;
    public readonly double YMax;

    public readonly double ZMin;
    public readonly double ZMax;

    private AxisLimits3d(double xMin, double xMax, double yMin, double yMax, double zMin, double zMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
        ZMin = zMin;
        ZMax = zMax;
    }

    public AxisLimits AxisLimits2d => new(XMin, XMax, YMin, YMax);

    public static AxisLimits3d FromPoints(IEnumerable<Coordinates3d> coordinates)
    {
        CoordinateRangeMutable xRange = new(double.MaxValue, double.MinValue);
        CoordinateRangeMutable yRange = new(double.MaxValue, double.MinValue);
        CoordinateRangeMutable zRange = new(double.MaxValue, double.MinValue);
        foreach (var c in coordinates)
        {
            xRange.Expand(c.X);
            yRange.Expand(c.Y);
            zRange.Expand(c.Z);
        }

        return new AxisLimits3d(xRange.Min, xRange.Max, yRange.Min, yRange.Max, zRange.Min, zRange.Max);
    }
}
