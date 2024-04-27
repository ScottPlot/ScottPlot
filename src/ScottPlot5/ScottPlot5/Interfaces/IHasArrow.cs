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
}
