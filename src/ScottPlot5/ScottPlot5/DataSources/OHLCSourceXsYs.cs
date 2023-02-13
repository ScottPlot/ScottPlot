using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.DataSources;

public class OHLCSourceXsYs : IOHLCSource
{
    private readonly IReadOnlyList<double> Xs;
    private readonly IReadOnlyList<OHLC> Ys;

    public OHLCSourceXsYs(IReadOnlyList<double> xs, IReadOnlyList<OHLC> ys)
    {
        Xs = xs;
        Ys = ys;
    }

    private Tuple<double, OHLC> GetCoordinatesAt(int index)
    {
        return new (Xs[index], Ys[index]);
    }

    public IReadOnlyList<Tuple<double, OHLC>> GetOHLCPoints()
    {
        return Enumerable.Range(0, Xs.Count).Select(i => GetCoordinatesAt(i)).ToArray();
    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        return new CoordinateRange(Xs.Min(), Xs.Max());
    }

    public CoordinateRange GetLimitsY()
    {
        // LINQ MinBy and MaxBy are not available on all targets
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;
        foreach(var y in Ys)
        {
            min = Math.Min(min, y.Low);
            max = Math.Max(max, y.High);
        }
        
        return new CoordinateRange(min,  max);
    }
}