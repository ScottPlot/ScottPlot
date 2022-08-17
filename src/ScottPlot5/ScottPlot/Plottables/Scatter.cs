/* Minimal case scatter plot for testing only
 * 
 * !! Avoid temptation to use generics or generic math at this early stage of development
 * 
 */

using SkiaSharp;

namespace ScottPlot.Plottables;

public class Scatter : PlottableBase
{
    public readonly DataSource.IScatterSource Data;

    public Color Color = new(0, 0, 255);
    public float LineWidth = 1;
    public float MarkerSize = 5;

    public override AxisLimits GetAxisLimits() => Data.GetLimits();

    public Scatter(DataSource.IScatterSource data)
    {
        Data = data;
    }

    private Pixel GetPixel(Coordinates coordinates, PixelRect dataRect)
    {
        float xPx = XAxis!.GetPixel(coordinates.X, dataRect);
        float yPx = YAxis!.GetPixel(coordinates.Y, dataRect);
        return new Pixel(xPx, yPx);
    }

    public override void Render(SKSurface surface, PixelRect dataRect)
    {
        if (XAxis is null || YAxis is null)
            throw new InvalidOperationException("Both axes must be set before first render");

        IEnumerable<Pixel> pixels = Data.GetScatterPoints().Select(x => GetPixel(x, dataRect));

        surface.Canvas.ClipRect(dataRect.ToSKRect());

        SKPaint paint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = Color.ToSKColor(),
            StrokeWidth = LineWidth,
        };

        // draw lines
        SKPath path = new();
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
}
