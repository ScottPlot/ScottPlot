namespace ScottPlot.DataSource;

public class SignalSource : ISignalSource
{
    private readonly IReadOnlyList<double> Ys;

    public double Period { get; set; }

    public double XOffset { get; set; }

    public SignalSource(IReadOnlyList<double> ys, double period)
    {
        Ys = ys;
        Period = period;
    }

    public CoordinateRange GetYRange(CoordinateRange xRange)
    {
        int i1 = GetIndex(xRange.Min, true);
        int i2 = GetIndex(xRange.Max, true);

        CoordinateRange yRange = new(Ys[i1], Ys[i1]);

        for (int i = i1; i <= i2; i++)
        {
            yRange.Expand(Ys[i]);
        }

        return yRange;
    }

    public int GetIndex(double x, bool clamp)
    {
        int i = (int)((x - XOffset) / Period);
        if (clamp)
        {
            i = Math.Max(i, 0);
            i = Math.Min(i, Ys.Count - 1);
        }
        return i;
    }

    public double GetX(int index)
    {
        return (index * Period) + XOffset;
    }

    public IReadOnlyList<double> GetYs()
    {
        return Ys;
    }

    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;
        for (int i = 0; i < Ys.Count; i++)
            rect.ExpandY(Ys[i]);
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
