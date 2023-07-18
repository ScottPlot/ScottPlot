using ScottPlot.Axis;
using System.Data;

namespace ScottPlot.Grids;

public class DefaultGrid : IGrid
{
    public LineStyle MajorLineStyle = new() { Width = 1, Color = Colors.Black.WithOpacity(.1) };
    public LineStyle MinorLineStyle = new() { Width = 0, Color = Colors.Black.WithOpacity(.05) };

    public int MaximumNumberOfGridLines = 1000;

    public bool IsBeneathPlottables { get; set; } = true;

    public IXAxis XAxis { get; private set; }
    public IYAxis YAxis { get; private set; }

    public DefaultGrid(IXAxis xAxis, IYAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }

    public void Replace(IXAxis xAxis)
    {
        XAxis = xAxis;
    }

    public void Replace(IYAxis yAxis)
    {
        YAxis = yAxis;
    }

    public void Render(RenderPack rp)
    {

        if (MinorLineStyle.Width > 0)
        {
            float[] xTicksMinor = XAxis.TickGenerator.Ticks.Where(x => !x.IsMajor).Select(x => XAxis.GetPixel(x.Position, rp.DataRect)).ToArray();
            float[] yTicksMinor = YAxis.TickGenerator.Ticks.Where(x => !x.IsMajor).Select(x => YAxis.GetPixel(x.Position, rp.DataRect)).ToArray();
            RenderGridLines(rp, xTicksMinor, XAxis.Edge, MinorLineStyle);
            RenderGridLines(rp, yTicksMinor, YAxis.Edge, MinorLineStyle);
        }

        if (MajorLineStyle.Width > 0)
        {
            float[] xTicksMajor = XAxis.TickGenerator.Ticks.Where(x => x.IsMajor).Select(x => XAxis.GetPixel(x.Position, rp.DataRect)).ToArray();
            float[] yTicksMajor = YAxis.TickGenerator.Ticks.Where(x => x.IsMajor).Select(x => YAxis.GetPixel(x.Position, rp.DataRect)).ToArray();
            RenderGridLines(rp, xTicksMajor, XAxis.Edge, MajorLineStyle);
            RenderGridLines(rp, yTicksMajor, YAxis.Edge, MajorLineStyle);
        }
    }

    private void RenderGridLines(RenderPack rp, float[] positions, Edge edge, LineStyle lineStyle)
    {
        Pixel[] starts = new Pixel[positions.Length];
        Pixel[] ends = new Pixel[positions.Length];

        for (int i = 0; i < positions.Length; i++)
        {
            float px = positions[i];
            starts[i] = edge.IsHorizontal() ? new Pixel(px, rp.DataRect.Bottom) : new Pixel(rp.DataRect.Left, px);
            ends[i] = edge.IsHorizontal() ? new Pixel(px, rp.DataRect.Top) : new Pixel(rp.DataRect.Right, px);
        }

        Drawing.DrawLines(rp.Canvas, starts, ends, lineStyle.Color, lineStyle.Width);
    }
}
