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
}
