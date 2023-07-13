/* Minimal case scatter plot for testing only.
 * Avoid temptation to use generics or generic math at this early stage of development!
 */

using ScottPlot.Axis;

namespace ScottPlot.Plottables;

public class Scatter : IPlottable
{
    public string? Label { get; set; }
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public LineStyle LineStyle { get; set; } = new();
    public MarkerStyle MarkerStyle { get; set; } = MarkerStyle.Default;
    public DataSources.IScatterSource Data { get; }

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

    public void Render(SKSurface surface)
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
            surface.Canvas.DrawPath(path, paint);
        }

        MarkerStyle.Render(surface.Canvas, pixels);
    }
}
