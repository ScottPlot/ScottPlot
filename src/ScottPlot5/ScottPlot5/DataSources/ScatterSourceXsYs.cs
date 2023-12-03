using System.Linq;

namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceXsYs : IScatterSource
{
    private readonly IReadOnlyList<double> Xs;
    private readonly IReadOnlyList<double> Ys;

    public ScatterSourceXsYs(IReadOnlyList<double> xs, IReadOnlyList<double> ys)
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
        if (!Xs.Any())
            return CoordinateRange.NotSet;

        return new CoordinateRange(Xs.Min(), Xs.Max());
    }

    public CoordinateRange GetLimitsY()
    {
        if (!Ys.Any())
            return CoordinateRange.NotSet;

        return new CoordinateRange(Ys.Min(), Ys.Max());
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        double maxDistanceSquared = maxDistance * maxDistance;
        double closestDistanceSquared = double.PositiveInfinity;

        int closestIndex = 0;
        double closestX = double.PositiveInfinity;
        double closestY = double.PositiveInfinity;

        for (int i = 0; i < Xs.Count; i++)
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
