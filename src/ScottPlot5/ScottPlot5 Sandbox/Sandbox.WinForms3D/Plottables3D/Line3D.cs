using Sandbox.WinForms3D.Primitives3D;
using ScottPlot;

namespace Sandbox.WinForms3D.Plottables3D;

public struct Line3D : IPlottable3D
{
    public Primitives3D.Line3D Line { get; }

    public LineStyle LineStyle { get; }

    public Line3D(Point3D start, Point3D end, float lineWidth = 1, ScottPlot.Color? color = null)
    {
        Line = new(start, end);

        LineStyle = new()
        {
            Width = lineWidth,
            Color = color ?? Colors.Black
        };
    }

    public void Render(RenderPack3D rp)
    {
        Pixel start = rp.GetPixel(Line.Start);
        Pixel end = rp.GetPixel(Line.End);
        PixelLine line = new(start, end);
        Drawing.DrawLine(rp.Canvas, rp.Paint, line, LineStyle);
    }
}
