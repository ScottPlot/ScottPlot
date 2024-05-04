namespace ScottPlot;

public interface IZoomRectangle
{
    bool VerticalSpan { get; set; }
    bool HorizontalSpan { get; set; }
    bool IsVisible { get; set; }
    Pixel MouseDown { get; set; }
    Pixel MouseUp { get; set; }
    void Apply(IXAxis xAxes);
    void Apply(IYAxis yAxes);
    void Render(RenderPack rp);
}
