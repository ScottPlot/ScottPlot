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

    private MouseInputState GetMouseState(MouseEventArgs e)
    {
        Pixel mousePosition = new(e.X, e.Y);

        List<MouseButton?> pressedButtons = new()
        {
            MouseButtonsPressed[MouseButton.Mouse1] ? MouseButton.Mouse1 : null,
            MouseButtonsPressed[MouseButton.Mouse2] ? MouseButton.Mouse2 : null,
            MouseButtonsPressed[MouseButton.Mouse3] ? MouseButton.Mouse3 : null,
        };

        return new MouseInputState(mousePosition, pressedButtons);
    }

    // TODO: store this in backend
    Dictionary<MouseButton, bool> MouseButtonsPressed = new()
    {
        { MouseButton.Mouse1, false },
        { MouseButton.Mouse2, false },
        { MouseButton.Mouse3, false },
        { MouseButton.UNKNOWN, false },
    };

    MouseButton Convert(MouseButtons button) => button switch
    {
        MouseButtons.Left => MouseButton.Mouse1,
        MouseButtons.Right => MouseButton.Mouse2,
        MouseButtons.Middle => MouseButton.Mouse3,
        _ => MouseButton.UNKNOWN,
    };

    private Key Convert(KeyEventArgs e) => e.KeyCode switch
    {
        Keys.ControlKey => Key.Ctrl,
        Keys.Menu => Key.Alt,
        Keys.ShiftKey => Key.Shift,
        _ => Key.UNKNOWN,
    };

    private void SKElement_MouseDown(object sender, MouseEventArgs e)
    {
        MouseButtonsPressed[Convert(e.Button)] = true;
        Backend.TriggerMouseDown(GetMouseState(e));
    }

    private void SKElement_MouseUp(object sender, MouseEventArgs e)
    {
        MouseButtonsPressed[Convert(e.Button)] = false;
        Backend.TriggerMouseUp(GetMouseState(e));
    }

    // TODO: all actions call their base action
    private void SKElement_MouseMove(object sender, MouseEventArgs e)
    {
        Backend.TriggerMouseMove(GetMouseState(e));
        base.OnMouseMove(e);
    }

    private void SKElement_DoubleClick(object sender, EventArgs e)
    {
        Backend.TriggerDoubleClick(MouseInputState.Empty);
    }

    private void SKElement_MouseWheel(object sender, MouseEventArgs e)
    {
        Backend.TriggerMouseWheel(GetMouseState(e), 0, e.Delta);
    }

    private void SKElement_KeyDown(object sender, KeyEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine(e.KeyCode);
        Backend.TriggerKeyDown(Convert(e));
    }

    private void SKElement_KeyUp(object sender, KeyEventArgs e)
    {
        Backend.TriggerKeyUp(Convert(e));
    }
}
