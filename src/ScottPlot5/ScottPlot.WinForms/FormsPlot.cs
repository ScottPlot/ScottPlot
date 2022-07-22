using ScottPlot.Control;
using SkiaSharp.Views.Desktop;
using System;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public class FormsPlot : UserControl, IPlotControl
{
    readonly SKGLControl SKElement;

    public Plot Plot { get; } = new();

    public Backend Backend { get; private set; }

    public FormsPlot()
    {
        Backend = new(this);

        SKElement = new() { Dock = DockStyle.Fill, VSync = true };
        SKElement.PaintSurface += SKControl_PaintSurface;
        SKElement.MouseDown += SKElement_MouseDown;
        SKElement.MouseUp += SKElement_MouseUp;
        SKElement.MouseMove += SKElement_MouseMove;
        SKElement.DoubleClick += SKElement_DoubleClick;
        SKElement.MouseWheel += SKElement_MouseWheel;
        SKElement.KeyDown += SKElement_KeyDown;
        SKElement.KeyUp += SKElement_KeyUp;

        Controls.Add(SKElement);
    }

    public override void Refresh()
    {
        SKElement.Invalidate();
        base.Refresh();
    }

    private void SKControl_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        Plot.Render(e.Surface);
    }

    private void SKElement_MouseDown(object sender, MouseEventArgs e)
    {
        Backend.MouseDown(e.Pixel(), e.Button());
        base.OnMouseDown(e);
    }

    private void SKElement_MouseUp(object sender, MouseEventArgs e)
    {
        Backend.MouseUp(e.Pixel(), e.Button());
        base.OnMouseUp(e);
    }

    private void SKElement_MouseMove(object sender, MouseEventArgs e)
    {
        Backend.MouseMove(e.Pixel());
        base.OnMouseMove(e);
    }

    private void SKElement_DoubleClick(object sender, EventArgs e)
    {
        Backend.DoubleClick();
        base.OnDoubleClick(e);
    }

    private void SKElement_MouseWheel(object sender, MouseEventArgs e)
    {
        Backend.MouseWheel(e.Pixel(), e.Delta);
        base.OnMouseWheel(e);
    }

    private void SKElement_KeyDown(object sender, KeyEventArgs e)
    {
        Backend.KeyDown(e.Key());
        base.OnKeyDown(e);
    }

    private void SKElement_KeyUp(object sender, KeyEventArgs e)
    {
        Backend.KeyUp(e.Key());
        base.OnKeyUp(e);
    }
}
