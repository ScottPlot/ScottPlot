/* Minimal case scatter plot for testing only.
 * Avoid temptation to use generics or generic math at this early stage of development!
 */

namespace ScottPlot.Plottables;

public class Scatter : IPlottable
{
    public string? Label { get; set; }
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public LineStyle LineStyle { get; set; } = new();
    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default;
    public DataSources.IScatterSource Data { get; }
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

    public Scatter(DataSources.IScatterSource data)
    {
        Data = data;
    }

    public void Render(RenderPack rp)
    {
        IEnumerable<Pixel> pixels = Data.GetScatterPoints().Select(x => Axes.GetPixel(x));

        if (!pixels.Any())
            return;

        if (LineStyle.IsVisible)
        {
            using SKPaint paint = new() { IsAntialias = true };
            LineStyle.ApplyToPaint(paint);

            using SKPath path = new();
            path.MoveTo(pixels.First().X, pixels.First().Y);
            foreach (Pixel pixel in pixels)
            {
                path.LineTo(pixel.X, pixel.Y);
            }
            rp.Canvas.DrawPath(path, paint);
        }

        MarkerStyle.Render(rp.Canvas, pixels);
    }
}
