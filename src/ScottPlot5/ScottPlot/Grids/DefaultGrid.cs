using ScottPlot.Axis;
using SkiaSharp;
using System.Linq;

namespace ScottPlot.Grids;

public class DefaultGrid : IGrid
{
    public float LineWidth = 1;
    public Color LineColor = Colors.Black.WithAlpha(20);
    public bool IsBeneathPlottables { get; set; } = true;

    public readonly IXAxis XAxis;
    public readonly IYAxis YAxis;

    public DefaultGrid(IXAxis xAxis, IYAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        RenderGridLines(surface, dataRect, XAxis);
        RenderGridLines(surface, dataRect, YAxis);
    }

    private void RenderGridLines(SKSurface surface, PixelRect dataRect, IAxis axis)
    {
        // TODO: restrict to visible ticks and max 1000?
        Tick[] ticks = axis.TickGenerator.Ticks;
        Pixel[] starts = new Pixel[ticks.Length];
        Pixel[] ends = new Pixel[ticks.Length];

        for (int i = 0; i < ticks.Length; i++)
        {
            float px = axis.GetPixel(ticks[i].Position, dataRect);
            starts[i] = axis.Edge.IsHorizontal() ? new Pixel(px, dataRect.Bottom) : new Pixel(dataRect.Left, px);
            ends[i] = axis.Edge.IsHorizontal() ? new Pixel(px, dataRect.Top) : new Pixel(dataRect.Right, px);
        }

        Drawing.DrawLines(surface, starts, ends, LineColor, LineWidth);
    }
}
