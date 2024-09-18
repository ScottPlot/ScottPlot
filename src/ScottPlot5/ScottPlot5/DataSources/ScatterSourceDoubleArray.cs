namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceDoubleArray(double[] xs, double[] ys) : IScatterSource, IDataSource, IGetNearest
{
    private readonly double[] Xs = xs;
    private readonly double[] Ys = ys;

    public int MinRenderIndex { get; set; } = 0;
    public int MaxRenderIndex { get; set; } = int.MaxValue;
    private int RenderIndexCount => Math.Min(Ys.Length - 1, MaxRenderIndex) - MinRenderIndex + 1;
    bool IDataSource.PreferCoordinates => false;
    int IDataSource.Length => Math.Min(Xs.Length, Ys.Length);

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
        => DataSourceUtilities.GetNearest(this, mouseLocation, renderInfo, maxDistance);


    public DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
        => DataSourceUtilities.GetNearestX(this, mouseLocation, renderInfo, maxDistance);

    Coordinates IDataSource.GetCoordinate(int index) => new Coordinates(Xs[index], Ys[index]);
    Coordinates IDataSource.GetCoordinateScaled(int index) => new Coordinates(Xs[index], Ys[index]);
    double IDataSource.GetX(int index) => Xs[index];
    double IDataSource.GetXScaled(int index) => Xs[index];
    double IDataSource.GetY(int index) => Ys[index];
    double IDataSource.GetYScaled(int index) => Ys[index];
}
