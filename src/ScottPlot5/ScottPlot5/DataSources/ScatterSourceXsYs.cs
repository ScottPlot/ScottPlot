using System.Linq;

namespace ScottPlot.DataSources;

/// <summary>
/// This data source manages X/Y points as separate X and Y collections
/// </summary>
public class ScatterSourceXsYs : IScatterSource
{
    private readonly IReadOnlyList<double> Xs;
    private readonly IReadOnlyList<double> Ys;

    public ScatterSourceXsYs(IReadOnlyList<double> xs, IReadOnlyList<double> ys)
    {
        Xs = xs;
        Ys = ys;
    }

    private Coordinates GetCoordinatesAt(int index)
    {
        return new Coordinates(Xs[index], Ys[index]);
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return Xs.Zip(Ys, (x, y) => new Coordinates(x, y)).ToArray();

    }

    public AxisLimits GetLimits()
    {
        return new AxisLimits(GetLimitsX(), GetLimitsY());
    }

    public CoordinateRange GetLimitsX()
    {
        if (!Xs.Any())
            return CoordinateRange.NotSet;

        return new CoordinateRange(Xs.Min(), Xs.Max());
    }

    public CoordinateRange GetLimitsY()
    {
        if (!Ys.Any())
            return CoordinateRange.NotSet;

        return new CoordinateRange(Ys.Min(), Ys.Max());
    }
}
