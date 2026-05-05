using ScottPlot.ScottPlot3D.Primitives3D;

namespace ScottPlot.ScottPlot3D;

public class RenderPack3D(Plot3D plot3d, SKSurface surface) : IDisposable
{
    public Plot3D Plot { get; } = plot3d;
    public SKCanvas Canvas { get; } = surface.Canvas;
    public PixelRect ImageRect { get; } = surface.Canvas.LocalClipBounds.ToPixelRect();
    public Paint Paint { get; } = Paint.NewDisposablePaint();

    public Pixel GetPixel(Point3D point) => Plot.GetPoint2D(point, ImageRect.Center);

    public void Dispose()
    {
        Paint.Dispose();
        GC.SuppressFinalize(this);
    }
}
