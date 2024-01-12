using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class SignalConst : IPlottable
{
    readonly SegmentedTree<double> SegmentedTree = new();
    readonly double[] Ys;
    readonly double Period;

    public readonly MarkerStyle Marker = new();
    public readonly LineStyle LineStyle = new();

    public string? Label { get; set; }
    public double XOffset = 0;
    public double YOffset = 0;
    public int MinRenderIndex = 0;
    public int MaxRenderIndex = int.MaxValue;

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            Marker.Fill.Color = value;
            Marker.Outline.Color = value;
        }
    }

    public SignalConst(double[] ys, double period = 1)
    {
        Ys = ys;
        Period = period;
        SegmentedTree.SourceArray = ys;
        MaxRenderIndex = ys.Length - 1;
    }

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

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
        double min = double.PositiveInfinity;
        double max = double.NegativeInfinity;

        for (int i = firstIndex; i <= lastIndex; i++)
        {
            min = Math.Min(min, Ys[i]);
            max = Math.Max(max, Ys[i]);
        }

        return new SignalRangeY(min, max);
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

        // first and last Y vales for this column
        float yEnter = axes.GetPixelY(Ys[i1] + YOffset);
        float yExit = axes.GetPixelY(Ys[i2] + YOffset);

        // column min and max
        SignalRangeY rangeY = GetLimitsY(i1, i2);
        float yBottom = axes.GetPixelY(rangeY.Min + YOffset);
        float yTop = axes.GetPixelY(rangeY.Max + YOffset);

        return new PixelColumn(xPixel, yEnter, yExit, yBottom, yTop);
    }

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        IEnumerable<PixelColumn> cols = Enumerable.Range(0, (int)Axes.DataRect.Width)
            .Select(x => GetPixelColumn(Axes, x))
            .Where(x => x.HasData);

        if (!cols.Any())
            return;

        using SKPath path = new();
        path.MoveTo(cols.First().X, cols.First().Enter);

        foreach (PixelColumn col in cols)
        {
            path.LineTo(col.X, col.Enter);
            path.MoveTo(col.X, col.Bottom);
            path.LineTo(col.X, col.Top);
            path.MoveTo(col.X, col.Exit);
        }

        rp.Canvas.DrawPath(path, paint);
    }
}
