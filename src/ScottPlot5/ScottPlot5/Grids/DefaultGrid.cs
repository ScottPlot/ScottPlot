using ScottPlot.Axis;
using SkiaSharp;
using System.Data;
using System.Linq;

namespace ScottPlot.Grids;

public class DefaultGrid : IGrid
{
    public float MajorGridLineWidth = 1;
    public Color MajorGridLineColor = Colors.Black.WithOpacity(.1);
    public float MinorGridLineWidth = 0;
    public Color MinorGridLineColor = Colors.Black.WithOpacity(.05);
    public int MaximumNumberOfGridLines = 1000;

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

        if (MinorGridLineWidth > 0)
        {
            float[] xTicksMinor = XAxis.TickGenerator.Ticks.Where(x => !x.IsMajor).Select(x => XAxis.GetPixel(x.Position, dataRect)).ToArray();
            float[] yTicksMinor = YAxis.TickGenerator.Ticks.Where(x => !x.IsMajor).Select(x => YAxis.GetPixel(x.Position, dataRect)).ToArray();
            RenderGridLines(surface, dataRect, xTicksMinor, XAxis.Edge, MinorGridLineColor, MinorGridLineWidth);
            RenderGridLines(surface, dataRect, yTicksMinor, YAxis.Edge, MinorGridLineColor, MinorGridLineWidth);
        }

        if (MajorGridLineWidth > 0)
        {
            float[] xTicksMajor = XAxis.TickGenerator.Ticks.Where(x => x.IsMajor).Select(x => XAxis.GetPixel(x.Position, dataRect)).ToArray();
            float[] yTicksMajor = YAxis.TickGenerator.Ticks.Where(x => x.IsMajor).Select(x => YAxis.GetPixel(x.Position, dataRect)).ToArray();
            RenderGridLines(surface, dataRect, xTicksMajor, XAxis.Edge, MajorGridLineColor, MajorGridLineWidth);
            RenderGridLines(surface, dataRect, yTicksMajor, YAxis.Edge, MajorGridLineColor, MajorGridLineWidth);
        }
    }

    private void RenderGridLines(SKSurface surface, PixelRect dataRect, float[] positions, Edge edge, Color color, float lineWidth)
    {
        Pixel[] starts = new Pixel[positions.Length];
        Pixel[] ends = new Pixel[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            float px = positions[i];
            starts[i] = edge.IsHorizontal() ? new Pixel(px, dataRect.Bottom) : new Pixel(dataRect.Left, px);
            ends[i] = edge.IsHorizontal() ? new Pixel(px, dataRect.Top) : new Pixel(dataRect.Right, px);
        }

        Drawing.DrawLines(surface, starts, ends, color, lineWidth);
    }
}
