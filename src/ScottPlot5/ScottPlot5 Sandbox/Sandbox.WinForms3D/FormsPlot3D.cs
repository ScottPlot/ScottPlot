using Sandbox.WinForms3D.Primitives3D;
using SkiaSharp.Views.Desktop;
using System.Windows.Forms;

namespace Sandbox.WinForms3D;

public class FormsPlot3D : UserControl
{
    private SKControl SKControl { get; }
    public Plot3D Plot3D = new();

    Rotation3D? MouseDownRotation = null;
    double MouseDownZoom;
    Point MouseDownPoint;
    Point3D MouseDownCameraCenter;

    public FormsPlot3D()
    {
        SKControl = new() { Dock = DockStyle.Fill };
        SKControl.PaintSurface += (s, e) => Plot3D.Render(e.Surface);
        Controls.Add(SKControl);

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
    }

    public override void Refresh()
    {
        SKControl?.Invalidate();
        base.Refresh();
    }
}
