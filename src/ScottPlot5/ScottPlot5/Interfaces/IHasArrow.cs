namespace ScottPlot;

public interface IHasArrow
{
    ArrowStyle ArrowStyle { get; }
    Color ArrowColor { get; set; }
    float ArrowLineWidth { get; set; }
}
