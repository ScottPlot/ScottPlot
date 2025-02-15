using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ScottPlot.WPF;

internal static class WpfPlotExtensions
{
    internal static Pixel ToPixel(this MouseEventArgs e, FrameworkElement fe)
    {
        return fe.ToPixel(e.GetPosition(fe));
    }

    internal static Pixel ToPixel(this FrameworkElement fe, Point position)
    {
        DpiScale dpiScale = VisualTreeHelper.GetDpi(fe);
        return new Pixel((float)(position.X * dpiScale.DpiScaleX), (float)(position.Y * dpiScale.DpiScaleY));
    }

    internal static void ProcessMouseDown(this Interactivity.UserInputProcessor processor, FrameworkElement fe, MouseButtonEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);

        Interactivity.IUserAction action = e.ChangedButton switch
        {
            MouseButton.Left => new Interactivity.UserActions.LeftMouseDown(pixel),
            MouseButton.Middle => new Interactivity.UserActions.MiddleMouseDown(pixel),
            MouseButton.Right => new Interactivity.UserActions.RightMouseDown(pixel),
            _ => new Interactivity.UserActions.Unknown(e.ChangedButton.ToString(), "pressed"),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseUp(this Interactivity.UserInputProcessor processor, FrameworkElement fe, MouseButtonEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);

        Interactivity.IUserAction action = e.ChangedButton switch
        {
            MouseButton.Left => new Interactivity.UserActions.LeftMouseUp(pixel),
            MouseButton.Middle => new Interactivity.UserActions.MiddleMouseUp(pixel),
            MouseButton.Right => new Interactivity.UserActions.RightMouseUp(pixel),
            _ => new Interactivity.UserActions.Unknown(e.ChangedButton.ToString(), "released"),
        };

        processor.Process(action);
    }

    internal static void ProcessMouseMove(this Interactivity.UserInputProcessor processor, FrameworkElement fe, MouseEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);
        Interactivity.IUserAction action = new Interactivity.UserActions.MouseMove(pixel);
        processor.Process(action);
    }

    internal static void ProcessMouseWheel(this Interactivity.UserInputProcessor processor, FrameworkElement fe, MouseWheelEventArgs e)
    {
        Pixel pixel = e.ToPixel(fe);

        Interactivity.IUserAction action = e.Delta > 0
            ? new Interactivity.UserActions.MouseWheelUp(pixel)
            : new Interactivity.UserActions.MouseWheelDown(pixel);

        processor.Process(action);
    }

    internal static void ProcessKeyDown(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyDown(key);
        processor.Process(action);
    }

    internal static void ProcessKeyUp(this Interactivity.UserInputProcessor processor, KeyEventArgs e)
    {
        Interactivity.Key key = e.ToKey();
        Interactivity.IUserAction action = new Interactivity.UserActions.KeyUp(key);
        processor.Process(action);
    }

    internal static Interactivity.Key ToKey(this KeyEventArgs e)
    {
        Key key = e.Key == Key.System ? e.SystemKey : e.Key; // required to capture Alt

        return key switch
        {
            Key.LeftCtrl => Interactivity.StandardKeys.Control,
            Key.RightCtrl => Interactivity.StandardKeys.Control,
            Key.LeftAlt => Interactivity.StandardKeys.Alt,
            Key.RightAlt => Interactivity.StandardKeys.Alt,
            Key.LeftShift => Interactivity.StandardKeys.Shift,
            Key.RightShift => Interactivity.StandardKeys.Shift,
            _ => new Interactivity.Key(key.ToString()),
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

