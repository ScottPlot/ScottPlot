namespace ScottPlot;

public interface IHasShadow
{
    FillStyle ShadowFillStyle { get; }
    PixelOffset ShadowOffset { get; set; }
    Alignment ShadowAlignment { get; set; }
    Color ShadowColor { get; set; }
}
