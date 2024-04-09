namespace ScottPlot;

public struct ArrowStyle
{
    public LineStyle LineStyle { get; set; } = new();

    public ArrowAnchor Anchor { get; set; } = ArrowAnchor.Center;

    public ArrowStyle(LineStyle lineStyle, ArrowAnchor anchor)
    {
        LineStyle = lineStyle;
        Anchor = anchor;
    }

    public ArrowStyle()
    {
    }
}
