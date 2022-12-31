using ScottPlot.Axis;
using ScottPlot.Style;
using System.Data;

namespace ScottPlot.Grids;

public class DefaultGrid : IGrid
{
    public LineStyle MajorLineStyle = new() { Width = 1, Color = Colors.Black.WithOpacity(.1) };
    public LineStyle MinorLineStyle = new() { Width = 0, Color = Colors.Black.WithOpacity(.05) };

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

        if (MinorLineStyle.Width > 0)
        {
            float[] xTicksMinor = XAxis.TickGenerator.Ticks.Where(x => !x.IsMajor).Select(x => XAxis.GetPixel(x.Position, dataRect)).ToArray();
            float[] yTicksMinor = YAxis.TickGenerator.Ticks.Where(x => !x.IsMajor).Select(x => YAxis.GetPixel(x.Position, dataRect)).ToArray();
            RenderGridLines(surface, dataRect, xTicksMinor, XAxis.Edge, MinorLineStyle);
            RenderGridLines(surface, dataRect, yTicksMinor, YAxis.Edge, MinorLineStyle);
        }

        if (MajorLineStyle.Width > 0)
        {
            float[] xTicksMajor = XAxis.TickGenerator.Ticks.Where(x => x.IsMajor).Select(x => XAxis.GetPixel(x.Position, dataRect)).ToArray();
            float[] yTicksMajor = YAxis.TickGenerator.Ticks.Where(x => x.IsMajor).Select(x => YAxis.GetPixel(x.Position, dataRect)).ToArray();
            RenderGridLines(surface, dataRect, xTicksMajor, XAxis.Edge, MajorLineStyle);
            RenderGridLines(surface, dataRect, yTicksMajor, YAxis.Edge, MajorLineStyle);
        }
    }

    private void RenderGridLines(SKSurface surface, PixelRect dataRect, float[] positions, Edge edge, LineStyle lineStyle)
    {
        Pixel[] starts = new Pixel[positions.Length];
        Pixel[] ends = new Pixel[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            float px = positions[i];
            starts[i] = edge.IsHorizontal() ? new Pixel(px, dataRect.Bottom) : new Pixel(dataRect.Left, px);
            ends[i] = edge.IsHorizontal() ? new Pixel(px, dataRect.Top) : new Pixel(dataRect.Right, px);
        }

        Drawing.DrawLines(surface, starts, ends, lineStyle.Color, lineStyle.Width);
    }
}
