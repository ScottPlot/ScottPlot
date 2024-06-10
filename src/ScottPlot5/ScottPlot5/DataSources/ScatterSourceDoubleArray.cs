namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceDoubleArray(double[] xs, double[] ys) : IScatterSource
{
    private readonly double[] Xs = xs;
    private readonly double[] Ys = ys;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;
    private int RenderIndexCount => Math.Min(Ys.Length - 1, MaxRenderIndex) - MinRenderIndex + 1;

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return Enumerable
            .Range(MinRenderIndex, RenderIndexCount)
            .Select(i => new Coordinates(Xs[i], Ys[i]))
            .ToList();
    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        return CoordinateRange.MinMaxNan(Xs.Skip(MinRenderIndex).Take(RenderIndexCount));
    }

    public CoordinateRange GetLimitsY()
    {
        return CoordinateRange.MinMaxNan(Ys.Skip(MinRenderIndex).Take(RenderIndexCount));
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
            double dX = (Xs[i] - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = (Ys[i] - mouseLocation.Y) * renderInfo.PxPerUnitY;
            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = Xs[i];
                closestY = Ys[i];
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared
            ? new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double closestDistance = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i2 = 0; i2 < RenderIndexCount; i2++)
        {
            int i = MinRenderIndex + i2;
            double dX = Math.Abs(Xs[i] - mouseLocation.X) * renderInfo.PxPerUnitX;

            if (dX <= closestDistance)
            {
                closestDistance = dX;
                closestX = Xs[i];
                closestY = Ys[i];
                closestIndex = i;
            }
        }

        return closestDistance <= maxDistance
            ? new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }
}
