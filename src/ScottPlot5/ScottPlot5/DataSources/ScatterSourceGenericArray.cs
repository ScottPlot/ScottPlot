namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceGenericArray<T1, T2> : IScatterSource
{
    private readonly T1[] Xs;
    private readonly T2[] Ys;

    public ScatterSourceGenericArray(T1[] xs, T2[] ys)
    {
        if (xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have equal length");

        Xs = xs;
        Ys = ys;
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        // TODO: try to avoid calling this
        return Xs.Zip(Ys, (x, y) => NumericConversion.GenericToCoordinates(ref x, ref y)).ToArray();
    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        double[] values = NumericConversion.GenericToDoubleArray(Xs);
        return CoordinateRange.MinMaxNan(values);
    }

    public CoordinateRange GetLimitsY()
    {
        double[] values = NumericConversion.GenericToDoubleArray(Ys);
        return CoordinateRange.MinMaxNan(values);
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i = 0; i < Xs.Length; i++)
        {
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
}
