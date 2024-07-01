namespace ScottPlot.Maui;

internal static class MauiPlotExtensions
{
    internal static Pixel Pixel(this TappedEventArgs e, MauiPlot plot)
    {
        Point? position = e.GetPosition(null);

        if (position is null)
            return new(double.NaN, double.NaN);

        Point tmpPos = new Point(
                position.Value.X * plot.DisplayScale,
                position.Value.X * plot.DisplayScale
            );

        return tmpPos.ToPixel();
    }

    internal static Pixel ToPixel(this Point p)
    {
        return new Pixel((float)p.X, (float)p.Y);
    }

    internal static Point ToPoint(this Pixel p)
    {
        return new Point(p.X, p.Y);
    }

    internal static Control.MouseButton ToButton(this TappedEventArgs e, MauiPlot plot)
    {
        var props = e.GetPosition(plot);
        switch (e.Buttons)
        {
            case ButtonsMask.Primary:
                return Control.MouseButton.Left;
            case ButtonsMask.Secondary:
                return Control.MouseButton.Right;
            default:
                return Control.MouseButton.Unknown;
        }
    }

    /*internal static Control.Key Key(this KeyRoutedEventArgs e)
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
    }*/
}

