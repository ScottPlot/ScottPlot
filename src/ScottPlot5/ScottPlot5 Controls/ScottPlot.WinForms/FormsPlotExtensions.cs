using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ScottPlot.Control;

namespace ScottPlot.WinForms;

public static class FormsPlotExtensions
{
    internal static Pixel Pixel(this MouseEventArgs e)
    {
        return new Pixel(e.X, e.Y);
    }

    internal static Control.MouseButton Button(this MouseEventArgs e)
    {
        return e.Button switch
        {
            System.Windows.Forms.MouseButtons.Left => Control.MouseButton.Left,
            System.Windows.Forms.MouseButtons.Right => Control.MouseButton.Right,
            System.Windows.Forms.MouseButtons.Middle => Control.MouseButton.Middle,
            _ => Control.MouseButton.Unknown,
        };
    }

    internal static Key Key(this KeyEventArgs e)
    {
        return e.KeyCode switch
        {
            Keys.ControlKey => Control.Key.Ctrl,
            Keys.Menu => Control.Key.Alt,
            Keys.ShiftKey => Control.Key.Shift,
            _ => Control.Key.Unknown,
        };
    }

    internal static Bitmap GetBitmap(this Plot plot, int width, int height)
    {
        byte[] bytes = plot.GetImage(width, height).GetImageBytes();
        using MemoryStream ms = new(bytes);
        System.Drawing.Bitmap bmp = new(ms);
        return bmp;
    }

    public static Bitmap GetBitmap(this Image img)
    {
        using MemoryStream ms = new(img.GetImageBytes(ImageFormat.Bmp));
        return new Bitmap(ms);
    }
}
