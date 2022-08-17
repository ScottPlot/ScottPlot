namespace ScottPlot.DataSource;

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
        return Enumerable.Range(0, Xs.Count).Select(i => GetCoordinatesAt(i)).ToArray();
    }

    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;
        for (int i = 0; i < Xs.Count; i++)
            rect.Expand(Xs[i], Ys[i]);
        return rect;
    }

    public CoordinateRange GetLimitsX()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.XMin, rect.XMin);
    }

    public CoordinateRange GetLimitsY()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.YMin, rect.YMin);
    }
}
