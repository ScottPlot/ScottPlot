using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using ScottPlot;
using ScottPlot.Interactivity;
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

    private Pixel ToPixel(PointerEventArgs e)
    {
        float x = (float)e.GetPosition(this).X;
        float y = (float)e.GetPosition(this).Y;
        return new Pixel(x, y);
    }
    
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        MouseDownRotation = Plot3D.Rotation;
        MouseDownPoint = e.GetPosition(this);
        MouseDownZoom = Plot3D.ZoomFactor;
        MouseDownCameraCenter = Plot3D.CameraCenter;

        e.Pointer.Capture(this);
    }
    
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        PointerUpdateKind kind = e.GetCurrentPoint(this).Properties.PointerUpdateKind;

        if (kind == PointerUpdateKind.LeftButtonReleased)
        {
            MouseDownRotation = null;
        }

        e.Pointer.Capture(null);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {  
        if (MouseDownRotation is null)
            return;

        Point currentPoint = e.GetPosition(this);
        PointerPointProperties properties = e.GetCurrentPoint(this).Properties;
        double dX = currentPoint.X - MouseDownPoint.X;
        double dY = currentPoint.Y - MouseDownPoint.Y;

        Plot3D.Rotation = MouseDownRotation.Value;
        if (properties.IsLeftButtonPressed)
        {
            float rotateSensitivity = 0.2f;
            Plot3D.Rotation.DegreesY += -dX * rotateSensitivity;
            Plot3D.Rotation.DegreesX += dY * rotateSensitivity;
        }
        
        if (properties.IsMiddleButtonPressed)
        {
            float panSensitivity = 0.005f;
            Plot3D.CameraCenter.X = MouseDownCameraCenter.X - dX * panSensitivity;
            Plot3D.CameraCenter.Y = MouseDownCameraCenter.Y + dY * panSensitivity;
        }
        
        if (properties.IsRightButtonPressed)
        {
            float dMax = (float)Math.Max(dX, -dY);
            Plot3D.ZoomFactor = MouseDownZoom + dMax;
        }

        InvalidateVisual();
    }
}
