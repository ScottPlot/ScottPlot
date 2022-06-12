using ScottPlot.Axes;
using SkiaSharp;

namespace ScottPlot.Plottables;

public class DebugGrid : PlottableBase
{
    public double Spacing { get; set; } = 1;

    public override void Render(SKSurface surface, PixelRect dataRect)
    {
        if (XAxis is null || YAxis is null)
            throw new InvalidOperationException("Both axes must be set before first render");

        using SKPaint paint = new()
        {
            IsAntialias = true,
            IsStroke = true,
            StrokeWidth = 1,
        };

        int ticksFromOriginLeft = (int)(XAxis.Left / Spacing);
        int ticksFromOriginRight = (int)(XAxis.Right / Spacing);
        for (int i = ticksFromOriginLeft; i <= ticksFromOriginRight; i++)
        {
            double x = Spacing * i;
            if (!XAxis.Contains(x))
                continue;

            float xPixel = XAxis.GetPixel(x, dataRect);
            xPixel = (float)Math.Round(xPixel); // snap to nearest whole number to avoid anti-aliasing artifacts

            paint.Color = i == 0 ? SKColors.Black : SKColors.Black.WithAlpha(100);
            surface.Canvas.DrawLine(xPixel, dataRect.Bottom, xPixel, dataRect.Top, paint);
        }

        int ticksFromOriginBottom = (int)(YAxis.Bottom / Spacing);
        int ticksFromOriginTop = (int)(YAxis.Top / Spacing);
        for (int i = ticksFromOriginBottom; i <= ticksFromOriginTop; i++)
        {
            double y = Spacing * i;
            if (!YAxis.Contains(y))
                continue;

            float yPixel = YAxis.GetPixel(y, dataRect);
            yPixel = (float)Math.Round(yPixel); // snap to nearest whole number to avoid anti-aliasing artifacts

            paint.Color = i == 0 ? SKColors.Black : SKColors.Black.WithAlpha(50);
            surface.Canvas.DrawLine(dataRect.Left, yPixel, dataRect.Right, yPixel, paint);
        }
    }
}
