/* Minimal case scatter plot for testing only
 * 
 * !! Avoid temptation to use generics or generic math at this early stage of development
 * 
 */

using SkiaSharp;

namespace ScottPlot.Plottables;

public class Scatter : PlottableBase
{
    private readonly double[] Xs;
    private readonly double[] Ys;
    private int Count => Xs.Length;

    public SKColor Color = SKColors.Blue;
    public float LineWidth = 1;
    public float MarkerSize = 5;

    public override AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Xs.Min(), Xs.Max(), Ys.Min(), Ys.Max());
    }

    public Scatter(double[] xs, double[] ys)
    {
        if (xs.Length != ys.Length)
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have same length");

        Xs = xs;
        Ys = ys;
    }

    public override void Render(SKSurface surface, PixelRect dataRect)
    {
        if (XAxis is null || YAxis is null)
            throw new InvalidOperationException("Both axes must be set before first render");

        float[] xPx = new float[Count];
        float[] yPx = new float[Count];
        for (int i = 0; i < Count; i++)
        {
            xPx[i] = XAxis.GetPixel(Xs[i], dataRect);
            yPx[i] = YAxis.GetPixel(Ys[i], dataRect);
        }

        surface.Canvas.ClipRect(dataRect.ToSKRect());

        SKPaint paint = new()
        {
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            Color = Color,
            StrokeWidth = LineWidth,
        };

        // draw lines
        SKPath path = new();
        path.MoveTo(xPx[0], yPx[0]);
        for (int i = 1; i < Count; i++)
        {
            path.LineTo(xPx[i], yPx[i]);
        }
        surface.Canvas.DrawPath(path, paint);

        // draw markers
        paint.IsStroke = false;
        for (int i = 0; i < Count; i++)
        {
            surface.Canvas.DrawCircle(xPx[i], yPx[i], MarkerSize / 2, paint);
        }
    }
}
