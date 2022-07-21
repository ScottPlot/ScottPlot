using System.Windows.Forms;
using SkiaSharp.Views.Desktop;
using ScottPlot.Control;
using System.Collections.Generic;
using System;

namespace ScottPlot.WinForms;

public class FormsPlot : UserControl, IPlotControl
{
    readonly SKGLControl SKElement = new() { Dock = DockStyle.Fill, VSync = true };

    public Plot Plot { get; } = new();

    public Backend Backend { get; private set; }

    public Coordinates MouseCoordinates => Backend.MouseCoordinates;

    public FormsPlot()
    {
        Backend = new(this);
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

    private Pixel GetMousePosition(MouseEventArgs e) => new(e.X, e.Y);

    MouseButton GetMouseButton(MouseEventArgs e) => e.Button switch
    {
        MouseButtons.Left => MouseButton.Mouse1,
        MouseButtons.Right => MouseButton.Mouse2,
        MouseButtons.Middle => MouseButton.Mouse3,
        _ => MouseButton.UNKNOWN,
    };

    private Key GetKey(KeyEventArgs e) => e.KeyCode switch
    {
        Keys.ControlKey => Key.Ctrl,
        Keys.Menu => Key.Alt,
        Keys.ShiftKey => Key.Shift,
        _ => Key.UNKNOWN,
    };

    private void SKElement_MouseDown(object sender, MouseEventArgs e)
    {
        Backend.TriggerMouseDown(GetMousePosition(e), GetMouseButton(e));
        base.OnMouseDown(e);
    }

    private void SKElement_MouseUp(object sender, MouseEventArgs e)
    {
        Backend.TriggerMouseUp(GetMousePosition(e), GetMouseButton(e));
        base.OnMouseUp(e);
    }

    private void SKElement_MouseMove(object sender, MouseEventArgs e)
    {
        Backend.TriggerMouseMove(GetMousePosition(e));
        base.OnMouseMove(e);
    }

    private void SKElement_DoubleClick(object sender, EventArgs e)
    {
        Backend.TriggerDoubleClick();
        base.OnDoubleClick(e);
    }

    private void SKElement_MouseWheel(object sender, MouseEventArgs e)
    {
        Backend.TriggerMouseWheel(GetMousePosition(e), e.Delta);
        base.OnMouseWheel(e);
    }

    private void SKElement_KeyDown(object sender, KeyEventArgs e)
    {
        Backend.TriggerKeyDown(GetKey(e));
        base.OnKeyDown(e);
    }

    private void SKElement_KeyUp(object sender, KeyEventArgs e)
    {
        Backend.TriggerKeyUp(GetKey(e));
        base.OnKeyUp(e);
    }
}
