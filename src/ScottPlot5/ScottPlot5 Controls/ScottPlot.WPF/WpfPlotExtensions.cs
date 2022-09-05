using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ScottPlot.WPF;

internal static class WpfPlotExtensions
{
    internal static Pixel Pixel(this MouseEventArgs e, WpfPlot plot)
    {
        DpiScale dpiScale = VisualTreeHelper.GetDpi(plot);
        double x = e.GetPosition(plot).X * dpiScale.DpiScaleX;
        double y = e.GetPosition(plot).Y * dpiScale.DpiScaleY;
        return new Pixel((float)x, (float)y);
    }

    internal static Control.MouseButton ToButton(this MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Middle)
            return Control.MouseButton.Middle;
        else if (e.ChangedButton == MouseButton.Left)
            return Control.MouseButton.Left;
        else if (e.ChangedButton == MouseButton.Right)
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
}

