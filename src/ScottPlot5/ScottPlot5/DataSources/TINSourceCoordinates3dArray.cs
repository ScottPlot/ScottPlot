namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as a collection of coordinates
/// </summary>
public class TINSourceCoordinates3dArray(Coordinates3d[] coordinates) 
{
    private readonly Coordinates3d[] Coordinates = coordinates;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;
    private int RenderIndexCount => Math.Min(Coordinates.Length - 1, MaxRenderIndex) - MinRenderIndex + 1;

    public IReadOnlyList<Coordinates3d> GetTINPoints()
    {
        return Coordinates
            .Skip(MinRenderIndex)
            .Take(RenderIndexCount)
            .ToList();
    }

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new();
        IEnumerable<Coordinates3d> coords = Coordinates.Skip(MinRenderIndex).Take(RenderIndexCount);
        // extract 2d axis limits from 3d coordinates
        foreach (Coordinates3d coord in coords)
        {
            limits.Expand(coord.X, coord.Y);
        }
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

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        // TODO: Implement GetNearestX() in this DataSource
        // Code can be copied from ScatterSourceDoubleArray.GetNearestX() and modified as needed
        // Contributions are welcome!
        // https://github.com/ScottPlot/ScottPlot/issues/3807
        throw new NotImplementedException();
    }
}
