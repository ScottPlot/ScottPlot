using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class SignalConst<T>(T[] ys, double period) : IPlottable
    where T : struct, IComparable
{
    readonly SignalConstSourceArray<T> DataSource = new(ys, period);
    public readonly MarkerStyle Marker = new();
    public readonly LineStyle LineStyle = new();

    public string? Label { get; set; }

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

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits() => DataSource.GetAxisLimits();

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        IEnumerable<PixelColumn> cols = Enumerable.Range(0, (int)Axes.DataRect.Width)
            .Select(x => DataSource.GetPixelColumn(Axes, x))
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
