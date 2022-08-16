namespace ScottPlot.DataSource;

/// <summary>
/// This data source has evenly-spaced Xs and Ys stored in a fixed-length array.
/// Changes made to the contents of the array will appear when the plot is rendered.
/// </summary>
public class SignalArray : IDataSource, ISignalSource
{
    public double Period { get; set; }
    public double XOffset { get; set; }
    private readonly double[] Ys;

    public int Count => Ys.Length;

    public SignalArray(double[] ys, double period)
    {
        Ys = ys;
        Period = period;
    }

    public CoordinateRange GetMinMax(CoordinateRange xRange)
    {
        throw new NotImplementedException();
    }

    public int GetIndex(double x)
    {
        return (int)((x - XOffset) / Period);
    }

    public double GetX(int index)
    {
        return (index * Period) + XOffset;
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
}
