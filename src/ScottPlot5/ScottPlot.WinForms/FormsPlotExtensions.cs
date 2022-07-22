using System.Windows.Forms;
using ScottPlot.Control;

namespace ScottPlot.WinForms;

internal static class FormsPlotExtensions
{
    internal static Pixel Pixel(this MouseEventArgs e)
    {
        return new Pixel(e.X, e.Y);
    }

    internal static MouseButton Button(this MouseEventArgs e)
    {
        return e.Button switch
        {
            MouseButtons.Left => MouseButton.Left,
            MouseButtons.Right => MouseButton.Right,
            MouseButtons.Middle => MouseButton.Middle,
            _ => MouseButton.Unknown,
        };
    }

    internal static Key Key(this KeyEventArgs e)
    {
        return e.KeyCode switch
        {
            Keys.ControlKey => Control.Key.Ctrl,
            Keys.Menu => Control.Key.Alt,
            Keys.ShiftKey => Control.Key.Shift,
            _ => Control.Key.UNKNOWN,
        };
    }
}
