namespace ScottPlot.Plottables;

public class ImageMarker : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public ScottPlot.Image? Image { get; set; } = null;
    public Coordinates Location { get; set; }
    public float Scale { get; set; } = 1;
    public bool AntiAlias { get; set; } = true;
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.FromPoint(Location);

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible || Image is null)
            return;

        Pixel centerPixel = Axes.GetPixel(Location);
        float pxLeft = centerPixel.X - Image.Width / 2.0f * Scale;
        float pxRight = centerPixel.X + Image.Width / 2.0f * Scale;
        float pxBottom = centerPixel.Y + Image.Height / 2.0f * Scale;
        float pxTop = centerPixel.Y - Image.Height / 2.0f * Scale;
        PixelRect rect = new(pxLeft, pxRight, pxBottom, pxTop);

        using SKPaint paint = new();
        Image.Render(rp.Canvas, rect, paint, AntiAlias);
    }
}
