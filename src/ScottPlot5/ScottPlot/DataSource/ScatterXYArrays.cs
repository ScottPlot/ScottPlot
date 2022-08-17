namespace ScottPlot.DataSource;

public class ScatterXYArrays : IScatterSource
{
    private readonly double[] Xs;
    private readonly double[] Ys;

    public int Count => Xs.Length;

    public ScatterXYArrays(double[] xs, double[] ys)
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
        return Enumerable.Range(0, Xs.Length).Select(i => GetCoordinates(i)).ToArray();
    }

    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;

        for (int i = 0; i < Count; i++)
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
