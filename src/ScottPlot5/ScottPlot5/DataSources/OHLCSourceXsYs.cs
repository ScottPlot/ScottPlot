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
        return Xs.Zip(Ys, (x, y) => new Tuple<double, OHLC>(x, y)).ToArray();
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
        return new CoordinateRange(Ys.Min(y => y.Low),  Ys.Max(y => y.High));
    }
}