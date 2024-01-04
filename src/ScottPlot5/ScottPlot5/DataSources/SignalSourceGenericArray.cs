namespace ScottPlot.DataSources;

public class SignalSourceGenericArray<T> : ISignalSource
{
    private readonly T[] Ys;

    public double Period { get; set; }

    public double XOffset { get; set; }

    public double YOffset { get; set; }

    public SignalSourceGenericArray(T[] ys, double period)
    {
        Ys = ys;
        Period = period;
    }

    public int GetIndex(double x, bool clamp)
    {
        int i = (int)((x - XOffset) / Period);
        if (clamp)
        {
            i = Math.Max(i, 0);
            i = Math.Min(i, Ys.Length - 1);
        }
        return i;
    }

    private bool RangeContainsSignal(double xMin, double xMax)
    {
        int firstIndex = GetIndex(xMin, false);
        int lastIndex = GetIndex(xMax, false);
        return lastIndex >= 0 && firstIndex <= Ys.Length - 1;
    }

    public double GetX(int index)
    {
        return index * Period + XOffset;
    }

    public IReadOnlyList<double> GetYs()
    {
        return NumericConversion.GenericToDoubleArray(Ys);
    }

    public AxisLimits GetLimits()
    {
        double[] ys2 = NumericConversion.GenericToDoubleArray(Ys);

        return new AxisLimits(
            left: XOffset,
            right: (Ys.Length - 1) * Period + XOffset,
            bottom: ys2.Min() + YOffset,
            top: ys2.Max() + YOffset);
    }

    public CoordinateRange GetLimitsX()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.Left, rect.Left);
    }

    public CoordinateRange GetLimitsY()
    {
        CoordinateRect rect = GetLimits().Rect;
        return new CoordinateRange(rect.Bottom, rect.Bottom);
    }

    public PixelColumn GetPixelColumn(IAxes axes, int xPixelIndex)
    {
        float xPixel = axes.DataRect.Left + xPixelIndex;
        double xRangeMin = axes.GetCoordinateX(xPixel);
        float xUnitsPerPixel = (float)(axes.XAxis.Width / axes.DataRect.Width);
        double xRangeMax = xRangeMin + xUnitsPerPixel;

        if (RangeContainsSignal(xRangeMin, xRangeMax) == false)
            return PixelColumn.WithoutData(xPixel);

        // determine column limits horizontally
        int i1 = GetIndex(xRangeMin, true);
        int i2 = GetIndex(xRangeMax, true);
        float yEnter = axes.GetPixelY(NumericConversion.GenericToDouble(Ys, i1) + YOffset);
        float yExit = axes.GetPixelY(NumericConversion.GenericToDouble(Ys, i2) + YOffset);

        // determine column span vertically
        double yMin = double.PositiveInfinity;
        double yMax = double.NegativeInfinity;
        for (int i = i1; i <= i2; i++)
        {
            double value = NumericConversion.GenericToDouble(Ys, i);
            yMin = Math.Min(yMin, value);
            yMax = Math.Max(yMax, value);
        }
        yMin += YOffset;
        yMax += YOffset;

        float yBottom = axes.GetPixelY(yMin);
        float yTop = axes.GetPixelY(yMax);

        return new PixelColumn(xPixel, yEnter, yExit, yBottom, yTop);
    }
}
