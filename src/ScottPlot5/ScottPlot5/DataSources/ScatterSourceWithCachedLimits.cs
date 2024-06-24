namespace ScottPlot.DataSources;

public class CacheScatterLimitsDecorator(IScatterSource source) : IScatterSource
{
    private readonly IScatterSource _source = source;

    public int MinRenderIndex
    {
        get => _source.MinRenderIndex;
        set => _source.MinRenderIndex = value;
    }

    public int MaxRenderIndex
    {
        get => _source.MaxRenderIndex;
        set => _source.MaxRenderIndex = value;
    }

    private AxisLimits? _axisLimits = null;
    private CoordinateRange _limitsX = CoordinateRange.NotSet;
    private CoordinateRange _limitsY = CoordinateRange.NotSet;

    public AxisLimits GetLimits()
    {
        if (_axisLimits is null)
            _axisLimits = _source.GetLimits();

        return _axisLimits.Value;
    }

    public CoordinateRange GetLimitsX()
    {
        if (_limitsX == CoordinateRange.NotSet)
            _limitsX = _source.GetLimitsX();

        return _limitsX;
    }

    public CoordinateRange GetLimitsY()
    {
        if (_limitsY == CoordinateRange.NotSet)
            _limitsY = _source.GetLimitsY();

        return _limitsY;
    }

    public DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance = 15)
    {
        return _source.GetNearest(mouseLocation, renderInfo, maxDistance);
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return _source.GetScatterPoints();
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
