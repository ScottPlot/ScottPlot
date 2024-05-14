namespace ScottPlot.Plottables;

public class Benchmark : LabelStyleProperties, IPlottable
{
    public bool IsVisible { get; set; }

    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public override Label LabelStyle { get; set; } = new()
    {
        FontName = Fonts.Monospace,
        Alignment = Alignment.LowerLeft,
        BackgroundColor = Colors.Yellow,
        PixelPadding = new(3, 3, 6, 0),
        BorderWidth = 1,
        BorderColor = Colors.Black,
    };

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        LabelStyle.Text = $"Rendered in " +
            $"{rp.Elapsed.TotalMilliseconds:0.000} ms " +
            $"({1e3 / rp.Elapsed.TotalMilliseconds:N0} FPS)";

        using SKPaint paint = new();
        LabelStyle.Render(rp.Canvas, rp.DataRect.BottomLeft.WithOffset(10, -13), paint);
    }
}
