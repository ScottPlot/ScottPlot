namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceGenericList<T1, T2>(List<T1> xs, List<T2> ys) : IScatterSource, IDataSource, IGetNearest
{
    private readonly List<T1> Xs = xs;
    private readonly List<T2> Ys = ys;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;

    int IDataSource.Length => Math.Min(Xs.Count, Ys.Count);
    bool IDataSource.PreferCoordinates => false;
    bool IDataSource.IsSorted() => Xs.IsAscending(GenericComparer<T1>.Default);

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        List<Coordinates> points = new(this.GetRenderIndexCount());

        for (int i = MinRenderIndex; points.Count < points.Capacity; i++)
        {
            T1 x = Xs[i];
            T2 y = Ys[i];
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
        double[] values = NumericConversion.GenericToDoubleArray(Xs.Skip(MinRenderIndex).Take(this.GetRenderIndexCount()));
        return CoordinateRange.Extrema(values);
    }

    public CoordinateRange GetLimitsY()
    {
        double[] values = NumericConversion.GenericToDoubleArray(Ys.Skip(MinRenderIndex).Take(this.GetRenderIndexCount()));
        return CoordinateRange.Extrema(values);
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestSmart(this, mouseLocation, renderInfo, maxDistance);

    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestSmart(this, mouseLocation, renderInfo, maxDistance);

    int IDataSource.GetXClosestIndex(Coordinates mouseLocation) => DataSourceUtilities.GetClosestIndex(Xs, NumericConversion.DoubleToGeneric<T1>(mouseLocation.X), this.GetRenderIndexRange(), GenericComparer<T1>.Default);
    Coordinates IDataSource.GetCoordinate(int index) => new Coordinates(NumericConversion.GenericToDouble(Xs, index), NumericConversion.GenericToDouble(Ys, index));
    Coordinates IDataSource.GetCoordinateScaled(int index) => new Coordinates(NumericConversion.GenericToDouble(Xs, index), NumericConversion.GenericToDouble(Ys, index));
    double IDataSource.GetX(int index) => NumericConversion.GenericToDouble(Xs, index);
    double IDataSource.GetY(int index) => NumericConversion.GenericToDouble(Ys, index);
    double IDataSource.GetXScaled(int index) => NumericConversion.GenericToDouble(Xs, index);
    double IDataSource.GetYScaled(int index) => NumericConversion.GenericToDouble(Ys, index);

}
