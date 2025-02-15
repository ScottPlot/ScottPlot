namespace ScottPlot.DataSources;

public class VectorFieldDataSourceCoordinatesList(IList<RootedCoordinateVector> rootedVectors) : IVectorFieldSource, IDataSource, IGetNearest
{
    private readonly IList<RootedCoordinateVector> RootedVectors = rootedVectors;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;

    int IDataSource.Length => RootedVectors.Count;
    bool IDataSource.PreferCoordinates => true;

    public IReadOnlyList<RootedCoordinateVector> GetRootedVectors()
    {
        return RootedVectors
            .Skip(MinRenderIndex)
            .Take(this.GetRenderIndexCount())
            .ToList();
    }

    public AxisLimits GetLimits()
    {
        ExpandingAxisLimits limits = new();
        limits.Expand(RootedVectors
            .Select(v => v.Point)
            .Skip(MinRenderIndex)
            .Take(this.GetRenderIndexCount())
            );
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
        => DataSourceUtilities.GetNearest(this, mouseLocation, renderInfo, maxDistance);

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestX(this, mouseLocation, renderInfo, maxDistance);

    // To-DO : Validate!
    int IDataSource.GetXClosestIndex(Coordinates mouseLocation) => DataSourceUtilities.GetClosestIndex(RootedVectors, new RootedCoordinateVector(mouseLocation, default), this.GetRenderIndexRange(), BinarySearchComparer.Instance);
    Coordinates IDataSource.GetCoordinate(int index) => RootedVectors[index].Point;
    Coordinates IDataSource.GetCoordinateScaled(int index) => RootedVectors[index].Point;
    double IDataSource.GetX(int index) => RootedVectors[index].Point.X;
    double IDataSource.GetXScaled(int index) => RootedVectors[index].Point.X;
    double IDataSource.GetY(int index) => RootedVectors[index].Point.Y;
    double IDataSource.GetYScaled(int index) => RootedVectors[index].Point.Y;
    bool IDataSource.IsSorted() => RootedVectors.IsAscending(BinarySearchComparer.Instance);
}
