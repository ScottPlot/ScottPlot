using SkiaSharp;

namespace ScottPlot.Grids;

public class DefaultGrid : IGrid
{
    public float LineWidth = 1;
    public Color LineColor = Colors.Black.WithAlpha(20);

    public bool IsBeneathPlottables { get; set; } = true;

    public void Render(SKSurface surface, PixelRect dataRect, AxisViews.IAxisView axisView)
    {
        Tick[] ticks = axisView.GetVisibleTicks();
        Pixel[] starts = new Pixel[ticks.Length];
        Pixel[] ends = new Pixel[ticks.Length];

        for (int i = 0; i < ticks.Length; i++)
        {
            float px = axisView.Axis.GetPixel(ticks[i].Position, dataRect);
            starts[i] = axisView.Axis.IsHorizontal ? new Pixel(px, dataRect.Bottom) : new Pixel(dataRect.Left, px);
            ends[i] = axisView.Axis.IsHorizontal ? new Pixel(px, dataRect.Top) : new Pixel(dataRect.Right, px);
        }

        Drawing.DrawLines(surface, starts, ends, LineColor, LineWidth);
    }
}
