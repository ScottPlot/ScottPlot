namespace ScottPlot.Panels;

public class LegendPanel(Legend legend) : PanelBase
{
    public Legend Legend { get; } = legend;
    public Alignment Alignment { get; set; } = Alignment.MiddleCenter;
    public PixelPadding Padding { get; set; } = new(20, 20);

    public override float Measure(Paint paint)
    {
        if (Legend.LastRenderSize == PixelSize.NaN)
            Legend.GetImage();

        PixelSize size = Legend.LastRenderSize;
        return Edge.IsVertical()
            ? size.Width + Padding.Horizontal
            : size.Height + Padding.Vertical;
    }

    public override void Render(RenderPack rp, float size, float offset)
    {
        PixelRect rect = GetPanelRect(rp.DataRect, size, offset, rp.Paint);
        bool originalVisibility = Legend.IsVisible;
        Legend.IsVisible = true;
        Legend.Render(rp.Canvas, rp.Paint, rect, Alignment);
        Legend.IsVisible = originalVisibility;
    }
}
