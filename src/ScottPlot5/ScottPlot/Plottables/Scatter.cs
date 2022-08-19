/* Minimal case scatter plot for testing only
 * 
 * !! Avoid temptation to use generics or generic math at this early stage of development
 * 
 */

using ScottPlot.Axis;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class Scatter : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes2D Axes { get; set; } = Axes2D.Default;

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public readonly DataSource.IScatterSource Data;
    public Color Color = new(0, 0, 255);
    public float LineWidth = 1;
    public float MarkerSize = 5;

    public Scatter(DataSource.IScatterSource data)
    {
        Data = data;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        IEnumerable<Pixel> pixels = Data.GetScatterPoints().Select(x => Axes.GetPixel(x));

        surface.Canvas.ClipRect(dataRect.ToSKRect());

        using SKPaint paint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = Color.ToSKColor(),
            StrokeWidth = LineWidth,
        };

        // draw lines
        using SKPath path = new();
        path.MoveTo(pixels.First().X, pixels.First().Y);
        foreach (Pixel pixel in pixels)
        {
            path.LineTo(pixel.X, pixel.Y);
        }
        surface.Canvas.DrawPath(path, paint);

        // draw markers
        paint.IsStroke = false;
        foreach (Pixel pixel in pixels)
        {
            surface.Canvas.DrawCircle(pixel.X, pixel.Y, MarkerSize / 2, paint);
        }
    }

    public void Render(SKSurface surface)
    {
        throw new NotImplementedException();
    }
}
