namespace ScottPlot.DataSource;

/// <summary>
/// This data source has Xs and Ys defined in fixed-length arrays.
/// Changes made to the contents of the arrays will appear when the plot is rendered.
/// </summary>
public class ScatterXYLists : IScatterSource
{
    private readonly List<double> Xs;
    private readonly List<double> Ys;

    public int Count => Xs.Count;

    public ScatterXYLists(List<double> xs, List<double> ys)
    {
        Xs = xs;
        Ys = ys;
    }

    public Coordinates this[int index]
    {
        get => new(Xs[index], Ys[index]);
        set { Xs[index] = value.X; Ys[index] = value.Y; }
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

    public IEnumerator<Coordinates> GetEnumerator()
    {
        int i = 0;
        while (i < Count)
        {
            yield return new Coordinates(Xs[i], Ys[i]);
            i++;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
