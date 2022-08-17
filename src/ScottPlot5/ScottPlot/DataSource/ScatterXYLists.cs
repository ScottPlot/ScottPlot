namespace ScottPlot.DataSource;

public class ScatterXYLists : IScatterSource
{
    private readonly List<double> Xs;
    private readonly List<double> Ys;

    public ScatterXYLists(List<double> xs, List<double> ys)
    {
        Xs = xs;
        Ys = ys;
    }

    private Coordinates GetCoordinates(int index)
    {
        return new Coordinates(Xs[index], Ys[index]);
    }

    public IReadOnlyList<Coordinates> GetScatterPoints()
    {
        return Enumerable.Range(0, Xs.Count).Select(i => GetCoordinates(i)).ToArray();
    }

    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;

        for (int i = 0; i < Xs.Count; i++)
        {
            rect.ExpandX(Xs[i]);
            rect.ExpandY(Ys[i]);
        }

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
