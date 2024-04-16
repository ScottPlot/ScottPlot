namespace ScottPlot;

public interface IHasArrow
{
    ArrowStyle ArrowStyle { get; }
    ArrowAnchor ArrowAnchor { get; set; }
    Color ArrowColor { get; set; }
    float ArrowLineWidth { get; set; }
}
