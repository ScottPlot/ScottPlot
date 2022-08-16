namespace ScottPlot.DataSource;

/// <summary>
/// This data source has Xs and Ys defined in fixed-length arrays.
/// Changes made to the contents of the arrays will appear when the plot is rendered.
/// </summary>
public class XYArrays : IDataSource
{
    private readonly double[] Xs;
    private readonly double[] Ys;

    public int Count => Xs.Length;

    public XYArrays(double[] xs, double[] ys)
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
}
