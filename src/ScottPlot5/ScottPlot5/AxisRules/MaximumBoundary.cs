namespace ScottPlot.AxisRules;

public class MaximumBoundary(IXAxis xAxis, IYAxis yAxis, AxisLimits limits) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;
    public AxisLimits Limits { get; set; } = limits;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        double xSpan = Math.Min(XAxis.Span, Limits.HorizontalSpan);
        double ySpan = Math.Min(YAxis.Span, Limits.VerticalSpan);

        if (XAxis.Max > Limits.Right)
        {
            XAxis.Max = Limits.Right;
            XAxis.Min = Limits.Right - xSpan;
        }

        if (XAxis.Min < Limits.Left)
        {
            XAxis.Min = Limits.Left;
            XAxis.Max = Limits.Left + xSpan;
        }

        if (YAxis.Max > Limits.Top)
        {
            YAxis.Max = Limits.Top;
            YAxis.Min = Limits.Top - ySpan;
        }

        if (YAxis.Min < Limits.Bottom)
        {
            YAxis.Min = Limits.Bottom;
            YAxis.Max = Limits.Bottom + ySpan;
        }
    }
}
