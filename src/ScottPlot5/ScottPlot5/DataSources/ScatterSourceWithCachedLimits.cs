namespace ScottPlot.DataSources;

public class CacheScatterLimitsDecorator : IScatterSource
{
    private readonly IScatterSource _source;

    private AxisLimits? _axisLimits = null;
    private CoordinateRange? _limitsX = null;
    private CoordinateRange? _limitsY = null;

    public CacheScatterLimitsDecorator(IScatterSource source)
    {
        _source = source;
    }

    public AxisLimits GetLimits()
    {
        if (_axisLimits is null)
            _axisLimits = _source.GetLimits();

        return _axisLimits.Value;
    }

    public CoordinateRange GetLimitsX()
    {
        if (_limitsX is null)
            _limitsX = _source.GetLimitsX();

        return _limitsX;
    }

    public CoordinateRange GetLimitsY()
    {
        if (_limitsY is null)
            _limitsY = _source.GetLimitsY();

        return _limitsY;
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return _source.GetScatterPoints();
    }
}
