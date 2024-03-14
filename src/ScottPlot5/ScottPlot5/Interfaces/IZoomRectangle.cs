namespace ScottPlot;

public interface IZoomRectangle
{
    bool VerticalSpan { get; set; }
    bool HorizontalSpan { get; set; }
    bool IsVisible { get; set; }
    Pixel MouseDown { get; set; }
    Pixel MouseUp { get; set; }
    void Apply(Plot plot);
    void Render(RenderPack rp);
}
