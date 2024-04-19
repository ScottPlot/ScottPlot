namespace ScottPlot;

public interface IHasArrow
{
    ArrowStyle ArrowStyle { get; set; }
    Color ArrowColor { get; set; }
    float ArrowLineWidth { get; set; }
}
