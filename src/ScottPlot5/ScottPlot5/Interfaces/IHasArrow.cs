namespace ScottPlot;

public interface IHasArrow
{
    ArrowStyle ArrowStyle { get; set; }
    Color ArrowLineColor { get; set; }
    float ArrowLineWidth { get; set; }
    Color ArrowFillColor { get; set; }
    float ArrowMinimumLength { get; set; }
    float ArrowMaximumLength { get; set; }
    float ArrowOffset { get; set; }
    ArrowAnchor ArrowAnchor { get; set; }
    public float ArrowWidth { get; set; }
    public float ArrowheadAxisLength { get; set; }
    public float ArrowheadLength { get; set; }
    public float ArrowheadWidth { get; set; }
}
