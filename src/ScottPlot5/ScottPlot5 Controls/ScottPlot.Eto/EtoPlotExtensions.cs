using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Eto;

internal static class EtoPlotExtensions
{
    internal static Pixel Pixel(this MouseEventArgs e)
    {
        double x = e.Location.X;
        double y = e.Location.Y;
        return new Pixel((float)x, (float)y);
    }

    internal static Control.MouseButton ToButton(this MouseEventArgs e)
    {
        if (e.Buttons == MouseButtons.Middle)
            return Control.MouseButton.Middle;
        else if (e.Buttons == MouseButtons.Primary)
            return Control.MouseButton.Left;
        else if (e.Buttons == MouseButtons.Alternate)
            return Control.MouseButton.Right;
        else
            return Control.MouseButton.Unknown;
    }

    internal static Control.Key Key(this KeyEventArgs e)
    {
        // WPF likes to snatch Alt, in which case we have to grab the system key value
        var key = e.Key; // e.Key == System.Windows.Input.Key.System ? e.SystemKey : e.Key;

        return key switch
        {
            Keys.LeftControl => Control.Key.Ctrl,
            Keys.RightControl => Control.Key.Ctrl,
            Keys.LeftAlt => Control.Key.Alt,
            Keys.RightAlt => Control.Key.Alt,
            Keys.LeftShift => Control.Key.Shift,
            Keys.RightShift => Control.Key.Shift,
            _ => Control.Key.Unknown,
        };
    }
}
