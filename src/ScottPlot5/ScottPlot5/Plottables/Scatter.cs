/* Minimal case scatter plot for testing only.
 * Avoid temptation to use generics or generic math at this early stage of development!
 */

using ScottPlot.Axis;
using ScottPlot.Style;

namespace ScottPlot.Plottables;

public class Scatter : IPlottable
{
    public string? Label { get; set; }
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public float MarkerSize { get; set; } = 5;
    public Color MarkerColor { get; set; }
    public LineStyle LineStyle { get; set; } = new();
    public DataSource.IScatterSource Data { get; }

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public IEnumerable<LegendItem> LegendItems => EnumerableHelpers.One<LegendItem>(
        new LegendItem
        {
            Label = Label,
            Marker = CreateMarker(),
            Line = LineStyle,
        });

    private Marker CreateMarker()
    {
        return new Marker(MarkerShape.Circle, MarkerColor, MarkerSize);
    }

    public Scatter(DataSource.IScatterSource data)
    {
        Data = data;
    }

    public void Render(SKSurface surface)
    {
        IEnumerable<Pixel> pixels = Data.GetScatterPoints().Select(x => Axes.GetPixel(x));

        using SKPaint paint = LineStyle.MakePaint();

        using SKPath path = new();
        path.MoveTo(pixels.First().X, pixels.First().Y);
        foreach (Pixel pixel in pixels)
        {
            path.LineTo(pixel.X, pixel.Y);
        }
        surface.Canvas.DrawPath(path, paint);

        Marker marker = CreateMarker();
        Drawing.DrawMarkers(surface, marker, pixels);
    }
}
