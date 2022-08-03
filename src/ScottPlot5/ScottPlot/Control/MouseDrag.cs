namespace ScottPlot.Control;

public struct MouseDrag
{
    public readonly AxisLimits InitialLimits;
    public readonly Pixel From;
    public readonly Pixel To;

    public MouseDrag(AxisLimits limits, Pixel from, Pixel to)
    {
        InitialLimits = limits;
        From = from;
        To = to;
    }
}
