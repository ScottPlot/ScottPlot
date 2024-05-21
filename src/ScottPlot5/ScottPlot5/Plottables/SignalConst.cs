using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class SignalConst<T>(T[] ys, double period) : IPlottable, IHasLine, IHasMarker, IHasLegendText
    where T : struct, IComparable
{
    readonly SignalConstSource<T> Data = new(ys, period);

    public MarkerStyle MarkerStyle { get; set; } = new();
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public LineStyle LineStyle { get; set; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public int MinRenderIndex { get => Data.MinRenderIndex; set => Data.MinRenderIndex = value; }
    public int MaxRenderIndex { get => Data.MaxRenderIndex; set => Data.MaxRenderIndex = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineColor = value;
            MarkerFillColor = value;
            MarkerLineColor = value;
        }
    }

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        List<PixelColumn> cols = Data.GetPixelColumns(Axes);

        if (!cols.Any())
            return;

        using SKPath path = new();
        path.MoveTo(cols.First().X, cols.First().Enter);

        foreach (PixelColumn col in cols)
        {
            path.LineTo(col.X, col.Enter);
            if ((int)col.Top != (int)col.Bottom)
            {
                path.MoveTo(col.X, col.Bottom);
                path.LineTo(col.X, col.Top);
                path.MoveTo(col.X, col.Exit);
            }
        }

        rp.Canvas.DrawPath(path, paint);
    }
}
