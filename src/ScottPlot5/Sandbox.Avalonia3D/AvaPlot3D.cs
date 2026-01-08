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
using Key = Avalonia.Input.Key;

namespace Sandbox.Avalonia3D;

public class AvaPlot3D : UserControl
{
    public Plot3D Plot3D = new();

    Rotation3D? MouseDownRotation = null;
    double MouseDownZoom;
    Point MouseDownPoint;
    Point3D MouseDownCameraCenter;

    public AvaPlot3D()
    {
        Focusable = true; // Required for keyboard events
    }

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
        MouseDownRotation = Plot3D.Scene.Rotation;
        MouseDownPoint = e.GetPosition(this);
        MouseDownZoom = Plot3D.Scene.Zoom;
        MouseDownCameraCenter = Plot3D.Scene.Position;

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

    protected override void OnKeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                Plot3D.Scene.Rotation = new Rotation3D()
                {
                    DegreesX = Plot3D.Scene.Rotation.DegreesX,
                    DegreesY = Plot3D.Scene.Rotation.DegreesY + 5,
                    DegreesZ = Plot3D.Scene.Rotation.DegreesZ
                };
                break;
            case Key.S:
                Plot3D.Scene.Rotation = new Rotation3D()
                {
                    DegreesX = Plot3D.Scene.Rotation.DegreesX,
                    DegreesY = Plot3D.Scene.Rotation.DegreesY - 5,
                    DegreesZ = Plot3D.Scene.Rotation.DegreesZ
                };
                break;
            case Key.A:
                Plot3D.Scene.Rotation = new Rotation3D()
                {
                    DegreesX = Plot3D.Scene.Rotation.DegreesX + 5,
                    DegreesY = Plot3D.Scene.Rotation.DegreesY,
                    DegreesZ = Plot3D.Scene.Rotation.DegreesZ
                };
                break;
            case Key.D:
                Plot3D.Scene.Rotation = new Rotation3D()
                {
                    DegreesX = Plot3D.Scene.Rotation.DegreesX - 5,
                    DegreesY = Plot3D.Scene.Rotation.DegreesY,
                    DegreesZ = Plot3D.Scene.Rotation.DegreesZ
                };
                break;
            case Key.Q:
                Plot3D.Scene.Rotation = new Rotation3D()
                {
                    DegreesX = Plot3D.Scene.Rotation.DegreesX,
                    DegreesY = Plot3D.Scene.Rotation.DegreesY,
                    DegreesZ = Plot3D.Scene.Rotation.DegreesZ - 5,
                };
                break;
            case Key.E:
                Plot3D.Scene.Rotation = new Rotation3D()
                {
                    DegreesX = Plot3D.Scene.Rotation.DegreesX,
                    DegreesY = Plot3D.Scene.Rotation.DegreesY,
                    DegreesZ = Plot3D.Scene.Rotation.DegreesZ + 5,
                };
                break;
        }
        
        InvalidateVisual();
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        if (e.Delta.Y > 0)
            Plot3D.Scene.Zoom += 5;
        else if (e.Delta.Y < 0)
            Plot3D.Scene.Zoom -= 5;
        
        InvalidateVisual();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {  
        if (MouseDownRotation is null)
            return;

        Point currentPoint = e.GetPosition(this);
        PointerPointProperties properties = e.GetCurrentPoint(this).Properties;
        double dX = currentPoint.X - MouseDownPoint.X;
        double dY = currentPoint.Y - MouseDownPoint.Y;

        Plot3D.Scene.Rotation = MouseDownRotation.Value;
        if (properties.IsLeftButtonPressed)
        {
            float rotateSensitivity = 0.2f;
            Plot3D.Scene.Rotation = new Rotation3D()
            {
                DegreesX = Plot3D.Scene.Rotation.DegreesX - dX * rotateSensitivity,
                DegreesY = Plot3D.Scene.Rotation.DegreesY + dY * rotateSensitivity,
                DegreesZ = Plot3D.Scene.Rotation.DegreesZ
            };
        }
        
        if (properties.IsMiddleButtonPressed)
        {
            float panSensitivity = 0.005f;

            Plot3D.Scene.Position = new Point3D()
            {
                X = MouseDownCameraCenter.X - dX * panSensitivity,
                Y = MouseDownCameraCenter.Y + dY * panSensitivity,
                Z = MouseDownCameraCenter.Z
            };
        }
        
        if (properties.IsRightButtonPressed)
        {
            float dMax = (float)Math.Max(dX, -dY);
            Plot3D.Scene.Zoom = MouseDownZoom + dMax;
        }

        InvalidateVisual();
    }
}
