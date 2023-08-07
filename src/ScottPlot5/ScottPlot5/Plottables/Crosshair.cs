namespace ScottPlot.Plottables;

public class Crosshair : IPlottable
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public IEnumerable<LegendItem> LegendItems { get; } = Array.Empty<LegendItem>();

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public Coordinates Position { get; set; }

    public bool VerticalLineIsVisible { get; set; } = true;

    public bool HorizontalLineIsVisible { get; set; } = true;
    public readonly LineStyle LineStyle = new();

    public void Render(RenderPack rp)
    {
        PixelRect dataArea = rp.Canvas.LocalClipBounds.ToPixelRect();
        Pixel px = Axes.GetPixel(Position);

        using SKPaint paint = new();

        LineStyle.ApplyToPaint(paint);

        if (VerticalLineIsVisible)
        {
            rp.Canvas.DrawLine(px.X, dataArea.Top, px.X, dataArea.Bottom, paint);
        }

        if (HorizontalLineIsVisible)
        {
            rp.Canvas.DrawLine(dataArea.Left, px.Y, dataArea.Right, px.Y, paint);
        }
    }
}
