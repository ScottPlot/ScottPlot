/* Minimal case scatter plot for testing only.
 * Avoid temptation to use generics or generic math at this early stage of development!
 */

namespace ScottPlot.Plottables;

public class Scatter : IPlottable
{
    public string? Label { get; set; }
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public LineStyle LineStyle { get; set; } = new(); // TODO: hide this
    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default; // TODO: hide this

    public IScatterSource Data { get; }

    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.Fill.Color = value;
            MarkerStyle.Outline.Color = value;
        }
    }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One<LegendItem>(
        new LegendItem
        {
            Label = Label,
            Marker = MarkerStyle,
            Line = LineStyle,
        });

    public Scatter(IScatterSource data)
    {
        Data = data;
    }

    public void Render(RenderPack rp)
    {
        // TODO: can this be more effecient by moving this logic into the DataSource to avoid copying?
        IEnumerable<Pixel> pixels = Data.GetScatterPoints().Select(Axes.GetPixel);

        if (!pixels.Any())
            return;

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, pixels, LineStyle);
        Drawing.DrawMarkers(rp.Canvas, paint, pixels, MarkerStyle);
    }
}
