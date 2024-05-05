namespace ScottPlot.DataSources;

public class SignalConstSource<T>
    where T : struct, IComparable
{
    public readonly SegmentedTree<T> SegmentedTree = new();

    public readonly T[] Ys;
    public readonly double Period;

    public double XOffset = 0;
    public double YOffset = 0;
    public int MinRenderIndex = 0;
    public int MaxRenderIndex = int.MaxValue;

    public SignalConstSource(T[] ys, double period)
    {
        Ys = ys;
        Period = period;
        SegmentedTree.SourceArray = ys;
        MaxRenderIndex = ys.Length - 1;
    }
    public AxisLimits GetAxisLimits()
    {
        SegmentedTree.MinMaxRangeQuery(0, Ys.Length - 1, out double low, out double high);
        return new AxisLimits(0, Ys.Length * Period, low, high);
    }

    public int GetIndex(double x, bool visibleDataOnly)
    {
        int i = (int)((x - XOffset) / Period);

        if (visibleDataOnly)
        {
            i = Math.Max(i, MinRenderIndex);
            i = Math.Min(i, MaxRenderIndex);
        }

        return i;
    }

    public bool RangeContainsSignal(double xMin, double xMax)
    {
        int xMinIndex = GetIndex(xMin, false);
        int xMaxIndex = GetIndex(xMax, false);
        return xMaxIndex >= MinRenderIndex && xMinIndex <= MaxRenderIndex;
    }

    public SignalRangeY GetLimitsY(int firstIndex, int lastIndex)
    {
        SegmentedTree.MinMaxRangeQuery(firstIndex, lastIndex, out double min, out double max);
        return new(min, max);
    }

    public List<PixelColumn> GetPixelColumns(IAxes axes)
    {
        List<PixelColumn> cols = new();

        // ensure the same i1 isn't sampled twice
        int latIndex1 = int.MinValue;
        for (int xPixelIndex = 0; xPixelIndex < (int)axes.DataRect.Width; xPixelIndex++)
        {
            float xPixel = axes.DataRect.Left + xPixelIndex;
            double xRangeMin = axes.GetCoordinateX(xPixel);
            float xUnitsPerPixel = (float)(axes.XAxis.Width / axes.DataRect.Width);
            double xRangeMax = xRangeMin + xUnitsPerPixel;

            // off the edge of the data
            if (RangeContainsSignal(xRangeMin, xRangeMax) == false)
                continue;

            // determine column limits horizontally
            int i1 = GetIndex(xRangeMin, true);
            int i2 = GetIndex(xRangeMax, true);
            if (i1 == latIndex1)
                continue;
            latIndex1 = i1;

            // first and last Y vales for this column
            double y1 = NumericConversion.GenericToDouble(Ys, i1);
            double y2 = NumericConversion.GenericToDouble(Ys, i2);
            float yEnter = axes.GetPixelY(y1 + YOffset);
            float yExit = axes.GetPixelY(y2 + YOffset);

            // column min and max
            SignalRangeY rangeY = GetLimitsY(i1, i2);
            float yBottom = axes.GetPixelY(rangeY.Min + YOffset);
            float yTop = axes.GetPixelY(rangeY.Max + YOffset);

            cols.Add(new PixelColumn(xPixel, yEnter, yExit, yBottom, yTop));
        }

        return cols;
    }
}
