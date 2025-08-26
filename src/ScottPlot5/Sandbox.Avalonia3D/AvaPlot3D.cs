using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using ScottPlot.ScottPlot3D;
using ScottPlot.ScottPlot3D.Primitives3D;
using SkiaSharp;

namespace Sandbox.Avalonia3D;

public class AvaPlot3D : UserControl
{
    public Plot3D Plot3D = new();

    Rotation3D? MouseDownRotation = null;
    double MouseDownZoom;
    Point MouseDownPoint;
    Point3D MouseDownCameraCenter;

    private class CustomDrawOp : ICustomDrawOperation
    {
        private readonly Plot3D Plot3D;

        public Rect Bounds { get; }
        public bool HitTest(Point p) => true;
        public bool Equals(ICustomDrawOperation? other) => false;

        public CustomDrawOp(Rect bounds, Plot3D plot3D)
        {
            Plot3D = plot3D;
            Bounds = bounds;
        }

        public void Dispose()
        {
            // No-op
        }

        public void Render(ImmediateDrawingContext context)
        {
            var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
            if (leaseFeature is null) return;

            using var lease = leaseFeature.Lease();
            using SKAutoCanvasRestore _ = new(lease.SkCanvas, false);
            
            lease.SkCanvas.SaveLayer();
            Plot3D.Render(lease.SkSurface);
        }
    }
    
    public override void Render(DrawingContext context)
    {
        Rect controlBounds = new(Bounds.Size);
        CustomDrawOp customDrawOp = new(controlBounds, Plot3D);
        context.Custom(customDrawOp);
    }

    public AvaPlot3D()
    {
        /*
        SKControl.MouseDown += (s, e) =>
        {
            MouseDownRotation = Plot3D.Rotation;
            MouseDownPoint = e.Location;
            MouseDownZoom = Plot3D.ZoomFactor;
            MouseDownCameraCenter = Plot3D.CameraCenter;
        };

        SKControl.MouseUp += (s, e) =>
        {
            MouseDownRotation = null;
        };

        SKControl.MouseMove += (s, e) =>
        {
            if (MouseDownRotation is null)
                return;

            int dX = e.X - MouseDownPoint.X;
            int dY = e.Y - MouseDownPoint.Y;

            Plot3D.Rotation = MouseDownRotation.Value;
            if (e.Button == MouseButtons.Left)
            {
                float rotateSensitivity = 0.2f;
                Plot3D.Rotation.DegreesY += -dX * rotateSensitivity;
                Plot3D.Rotation.DegreesX += dY * rotateSensitivity;
            }
            else if (e.Button == MouseButtons.Middle)
            {
                float panSensitivity = 0.005f;
                Plot3D.CameraCenter.X = MouseDownCameraCenter.X - dX * panSensitivity;
                Plot3D.CameraCenter.Y = MouseDownCameraCenter.Y + dY * panSensitivity;
            }
            else if (e.Button == MouseButtons.Right)
            {
                float dMax = Math.Max(dX, -dY);
                Plot3D.ZoomFactor = MouseDownZoom + dMax;
            }

            Refresh();
        };
        */
    }
}
