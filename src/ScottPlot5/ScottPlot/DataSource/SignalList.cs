namespace ScottPlot.DataSource;

public class SignalList : ISignalSource
{
    public double Period { get; set; }
    public double XOffset { get; set; }
    public List<double> Ys { get; }

    public int Count => Ys.Count;

    double IReadOnlyList<double>.this[int index] => Ys[index];

    public SignalList(List<double> ys, double period)
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
            i = Math.Min(i, Count - 1);
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

    public Coordinates this[int index]
    {
        get => new(GetX(index), Ys[index]);
        set { Ys[index] = value.Y; }
    }

    public AxisLimits GetLimits()
    {
        AxisLimits rect = AxisLimits.NoLimits;

        for (int i = 0; i < Count; i++)
        {
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

    public IEnumerator<double> GetEnumerator()
    {
        int i = 0;
        while (i < Count)
        {
            yield return Ys[i];
            i++;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
