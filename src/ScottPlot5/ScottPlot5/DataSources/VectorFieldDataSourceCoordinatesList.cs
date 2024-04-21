namespace ScottPlot.DataSources;

public class VectorFieldDataSourceCoordinatesList(IList<RootedCoordinateVector> rootedVectors) : IVectorFieldSource
{
    private readonly IList<RootedCoordinateVector> RootedVectors = rootedVectors;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;
    private int RenderIndexCount => Math.Min(RootedVectors.Count - 1, MaxRenderIndex) - MinRenderIndex + 1;

    public IReadOnlyList<RootedCoordinateVector> GetRootedVectors()
    {
        return RootedVectors
            .Skip(MinRenderIndex)
            .Take(RenderIndexCount)
            .ToList();
    }

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new();
        limits.Expand(RootedVectors.Select(v => v.Point).Skip(MinRenderIndex).Take(RenderIndexCount));
        return limits.AxisLimits;
    }

    public CoordinateRange GetLimitsX()
    {
        return GetLimits().Rect.XRange;
    }

    public CoordinateRange GetLimitsY()
    {
        return GetLimits().Rect.YRange;
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
            double dX = (RootedVectors[i].Point.X - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = (RootedVectors[i].Point.Y - mouseLocation.Y) * renderInfo.PxPerUnitY;
            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = RootedVectors[i].Point.X;
                closestY = RootedVectors[i].Point.Y;
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared
            ? new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }
}
