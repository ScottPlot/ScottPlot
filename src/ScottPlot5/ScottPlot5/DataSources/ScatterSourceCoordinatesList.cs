namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as a collection of coordinates
/// </summary>
public class ScatterSourceCoordinatesList(List<Coordinates> coordinates) : IScatterSource, IDataSource, IGetNearest
{
    private readonly List<Coordinates> Coordinates = coordinates;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;

    bool IDataSource.PreferCoordinates => true;
    int IDataSource.Length => Coordinates.Count;

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return Coordinates
            .Skip(MinRenderIndex)
            .Take(this.GetRenderIndexCount())
            .ToList();
    }

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new();
        limits.Expand(Coordinates.Skip(MinRenderIndex).Take(this.GetRenderIndexCount()));
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
        => DataSourceUtilities.GetNearestSmart(this, mouseLocation, renderInfo, maxDistance);

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestXSmart(this, mouseLocation, renderInfo, maxDistance);

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation) => DataSourceUtilities.GetClosestIndex(Coordinates, mouseLocation, this.GetRenderIndexRange());
    Coordinates IDataSource.GetCoordinate(int index) => Coordinates[index];
    Coordinates IDataSource.GetCoordinateScaled(int index) => Coordinates[index];
    double IDataSource.GetX(int index) => Coordinates[index].X;
    double IDataSource.GetXScaled(int index) => Coordinates[index].X;
    double IDataSource.GetY(int index) => Coordinates[index].Y;
    double IDataSource.GetYScaled(int index) => Coordinates[index].Y;

    bool IDataSource.IsSorted() => Coordinates.IsAscending(BinarySearchComparer.Instance);
}
