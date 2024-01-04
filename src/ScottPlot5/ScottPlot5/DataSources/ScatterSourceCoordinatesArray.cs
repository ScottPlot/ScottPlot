namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as a collection of coordinates
/// </summary>
public class ScatterSourceCoordinatesArray : IScatterSource
{
    private readonly Coordinates[] Coordinates;

    public ScatterSourceCoordinatesArray(Coordinates[] coordinates)
    {
        Coordinates = coordinates;
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        // TODO: try to avoid calling this
        return Coordinates;
    }

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new();
        limits.Expand(Coordinates);
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

        for (int i = 0; i < Coordinates.Length; i++)
        {
            double dX = (Coordinates[i].X - mouseLocation.X) * renderInfo.PxPerUnitX;
            double dY = (Coordinates[i].Y - mouseLocation.Y) * renderInfo.PxPerUnitY;
            double distanceSquared = dX * dX + dY * dY;

            if (distanceSquared <= closestDistanceSquared)
            {
                closestDistanceSquared = distanceSquared;
                closestX = Coordinates[i].X;
                closestY = Coordinates[i].Y;
                closestIndex = i;
            }
        }

        return closestDistanceSquared <= maxDistanceSquared
            ? new DataPoint(closestX, closestY, closestIndex)
            : DataPoint.None;
    }
}
