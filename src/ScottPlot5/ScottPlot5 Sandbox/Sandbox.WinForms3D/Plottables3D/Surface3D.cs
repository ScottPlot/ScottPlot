using Sandbox.WinForms3D.Primitives3D;
using ScottPlot;

namespace Sandbox.WinForms3D.Plottables3D;

public class Surface3D(Point3D[] vertices) : IPlottable3D
{
    public Point3D[] Vertices { get; } = vertices;

    public readonly FillStyle FillStyle = new()
    {
        IsVisible = true,
        Color = Colors.Blue.WithAlpha(.2),
    };

    public readonly LineStyle LineStyle = new()
    {
        IsVisible = true,
        Color = Colors.Black,
        Width = 1,
    };

    public void Render(RenderPack3D rp)
    {
        if (Vertices.Length < 2)
            return;

        Pixel[] pixels = Vertices.Select(rp.GetPixel).ToArray();
        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, FillStyle);
        Drawing.DrawPath(rp.Canvas, rp.Paint, pixels, LineStyle);
    }
}
