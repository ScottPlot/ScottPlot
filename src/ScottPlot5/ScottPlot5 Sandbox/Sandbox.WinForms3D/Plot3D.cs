using Sandbox.WinForms3D.Plottables3D;
using Sandbox.WinForms3D.Primitives3D;
using ScottPlot;
using SkiaSharp;

namespace Sandbox.WinForms3D;

public class Plot3D
{
    public double ZoomFactor = 200;
    public Rotation3D Rotation = new() { DegreesX = 110, DegreesY = 18, DegreesZ = 5 };
    public Point3D CameraCenter = new();
    public readonly Axis3D Axis3D = new();
    public readonly List<IPlottable3D> Plottables = [];

    public Pixel GetPoint2D(Point3D point, Pixel imageCenter)
    {
        point = point.WithZoom(ZoomFactor);
        point = point.Translated(Rotation.CenterPoint, Point3D.Origin);
        point = point.RotatedX(Rotation.DegreesX);
        point = point.RotatedY(Rotation.DegreesY);
        point = point.RotatedZ(Rotation.DegreesZ);
        point = point.Translated(Point3D.Origin, Rotation.CenterPoint);
        float x = (float)(point.X - CameraCenter.X * ZoomFactor) + imageCenter.X;
        float y = (float)(CameraCenter.Y * ZoomFactor - point.Y) + imageCenter.Y;
        return new Pixel(x, y);
    }

    public void Render(SKSurface surface)
    {
        RenderPack3D rp = new(this, surface);

        Drawing.FillRectangle(rp.Canvas, rp.ImageRect, Colors.White);

        Axis3D.Render(rp);

        foreach (IPlottable3D plottable in Plottables)
        {
            plottable.Render(rp);
        }
    }
}
