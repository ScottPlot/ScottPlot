namespace ScottPlot.Plottables;

public class ImageRect() : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public Image? Image { get; set; } = null;
    public CoordinateRect Rect { get; set; } = new(0, 1, 0, 1);
    public bool AntiAlias { get; set; } = true;
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => new(Rect);

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible || Image is null)
            return;

        PixelRect pxRect = Axes.GetPixelRect(Rect);

        using SKPaint paint = new();
        Image.Render(rp.Canvas, pxRect, paint, AntiAlias);
    }
}
