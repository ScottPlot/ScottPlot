using Windows.System;
using Windows.Foundation;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Input;

namespace ScottPlot.WinUI;

internal static class WinUIPlotExtensions
{
    internal static Pixel Pixel(this PointerRoutedEventArgs e, WinUIPlot plot)
    {
        Point position = e.GetCurrentPoint(plot).Position;
        position.X *= plot.DisplayScale;
        position.Y *= plot.DisplayScale;
        return position.ToPixel();
    }

    internal static Pixel ToPixel(this Point p)
    {
        return new Pixel((float)p.X, (float)p.Y);
    }

    internal static Point ToPoint(this Pixel p)
    {
        return new Point(p.X, p.Y);
    }

    internal static Control.MouseButton ToButton(this PointerRoutedEventArgs e, WinUIPlot plot)
    {
        var props = e.GetCurrentPoint(plot).Properties;
        switch (props.PointerUpdateKind)
        {
            case PointerUpdateKind.MiddleButtonPressed:
            case PointerUpdateKind.MiddleButtonReleased:
                return Control.MouseButton.Middle;
            case PointerUpdateKind.LeftButtonPressed:
            case PointerUpdateKind.LeftButtonReleased:
                return Control.MouseButton.Left;
            case PointerUpdateKind.RightButtonPressed:
            case PointerUpdateKind.RightButtonReleased:
                return Control.MouseButton.Right;
            default:
                return Control.MouseButton.Unknown;
        }
    }

    internal static Control.Key Key(this KeyRoutedEventArgs e)
    {
        return e.Key switch
        {
            VirtualKey.Control => Control.Key.Ctrl,
            VirtualKey.LeftControl => Control.Key.Ctrl,
            VirtualKey.RightControl => Control.Key.Ctrl,

            VirtualKey.Menu => Control.Key.Alt,
            VirtualKey.LeftMenu => Control.Key.Alt,
            VirtualKey.RightMenu => Control.Key.Alt,

            VirtualKey.Shift => Control.Key.Shift,
            VirtualKey.LeftShift => Control.Key.Shift,
            VirtualKey.RightShift => Control.Key.Shift,

            _ => Control.Key.Unknown,
        };
    }
}

