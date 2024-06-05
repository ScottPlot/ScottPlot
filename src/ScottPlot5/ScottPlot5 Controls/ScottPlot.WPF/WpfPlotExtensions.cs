using ScottPlot.Control;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScottPlot.WPF;

internal static class WpfPlotExtensions
{
    internal static Pixel Pixel(this MouseEventArgs e, FrameworkElement fe)
    {
        return fe.Pixel(e.GetPosition(fe));
    }
    internal static Pixel Pixel(this FrameworkElement fe, Point position)
    {
        DpiScale dpiScale = VisualTreeHelper.GetDpi(fe);
        return new Pixel((float)(position.X * dpiScale.DpiScaleX), (float)(position.Y * dpiScale.DpiScaleY));
    }

    internal static Control.MouseButton ToButton(this MouseButtonEventArgs e)
    {
        if (e.ChangedButton == System.Windows.Input.MouseButton.Middle)
            return Control.MouseButton.Middle;
        else if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            return Control.MouseButton.Left;
        else if (e.ChangedButton == System.Windows.Input.MouseButton.Right)
            return Control.MouseButton.Right;
        else
            return Control.MouseButton.Unknown;
    }

    internal static Control.Key Key(this KeyEventArgs e)
    {
        // WPF likes to snatch Alt, in which case we have to grab the system key value
        var key = e.Key == System.Windows.Input.Key.System ? e.SystemKey : e.Key;

        return key switch
        {
            System.Windows.Input.Key.LeftCtrl => Control.Key.Ctrl,
            System.Windows.Input.Key.RightCtrl => Control.Key.Ctrl,
            System.Windows.Input.Key.LeftAlt => Control.Key.Alt,
            System.Windows.Input.Key.RightAlt => Control.Key.Alt,
            System.Windows.Input.Key.LeftShift => Control.Key.Shift,
            System.Windows.Input.Key.RightShift => Control.Key.Shift,
            _ => Control.Key.Unknown,
        };
    }

    internal static BitmapImage GetBitmapImage(this Plot plot, int width, int height)
    {
        byte[] bytes = plot.GetImageBytes(width, height);
        using MemoryStream ms = new(bytes);

        BitmapImage bmp = new();
        bmp.BeginInit();
        bmp.StreamSource = ms;
        bmp.EndInit();
        bmp.Freeze();

        return bmp;
    }
}

