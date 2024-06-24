namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceGenericArray<T1, T2>(T1[] xs, T2[] ys) : IScatterSource
{
    private readonly T1[] Xs = xs;
    private readonly T2[] Ys = ys;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;
    private int RenderIndexCount => Math.Min(Ys.Length - 1, MaxRenderIndex) - MinRenderIndex + 1;

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        List<Coordinates> points = new(RenderIndexCount);

        for (int i = 0; i < RenderIndexCount; i++)
        {
            T1 x = Xs[MinRenderIndex + i];
            T2 y = Ys[MinRenderIndex + i];
            Coordinates c = NumericConversion.GenericToCoordinates(ref x, ref y);
            points.Add(c);
        }

        return points;
    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        double[] values = NumericConversion.GenericToDoubleArray(Xs.Skip(MinRenderIndex).Take(RenderIndexCount));
        return CoordinateRange.MinMaxNan(values);
    }

    public CoordinateRange GetLimitsY()
    {
        double[] values = NumericConversion.GenericToDoubleArray(Ys.Skip(MinRenderIndex).Take(RenderIndexCount));
        return CoordinateRange.MinMaxNan(values);
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i2 = 0; i2 < RenderIndexCount; i2++)
        {
            int i = MinRenderIndex + i2;
            T1 xValue = Xs[i];
            T2 yValue = Ys[i];
            double xValueDouble = NumericConversion.GenericToDouble(ref xValue);
            double yValueDouble = NumericConversion.GenericToDouble(ref yValue);
            double dX = (xValueDouble - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = (yValueDouble - mouseLocation.Y) * renderInfo.PxPerUnitY;
            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = xValueDouble;
                closestY = yValueDouble;
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared
            ? new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        // TODO: Implement GetNearestX() in this DataSource
        // Code can be copied from ScatterSourceDoubleArray.GetNearestX() and modified as needed
        // Contributions are welcome!
        // https://github.com/ScottPlot/ScottPlot/issues/3807
        throw new NotImplementedException();
    }
}
