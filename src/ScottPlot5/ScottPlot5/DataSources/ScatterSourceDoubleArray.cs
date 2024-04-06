namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceDoubleArray : IScatterSource
{
    private readonly double[] Xs;
    private readonly double[] Ys;

    public ScatterSourceDoubleArray(double[] xs, double[] ys)
    {
        Xs = xs;
        Ys = ys;
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        // TODO: try to avoid calling this
        return Xs.Zip(Ys, (x, y) => new Coordinates(x, y)).ToArray();
    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        return CoordinateRange.MinMaxNan(Xs);
    }

    public CoordinateRange GetLimitsY()
    {
        return CoordinateRange.MinMaxNan(Ys);
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
}
