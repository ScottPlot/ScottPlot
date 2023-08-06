namespace ScottPlot.Control;

public struct MouseDrag
{
    public readonly MultiAxisLimits InitialLimits;
    public readonly Pixel From;
    public readonly Pixel To;

    public MouseDrag(MultiAxisLimits limits, Pixel from, Pixel to)
    {
        InitialLimits = limits;
        From = from;
        To = to;
    }
}
