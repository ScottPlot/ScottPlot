namespace ScottPlot.AxisRules;

public class MinimumBoundary(IXAxis xAxis, IYAxis yAxis, AxisLimits limits) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;
    public AxisLimits Limits { get; set; } = limits;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        double xSpan = Math.Max(XAxis.Range.Span, Limits.HorizontalSpan);
        double ySpan = Math.Max(YAxis.Range.Span, Limits.VerticalSpan);

        if (XAxis.Range.Max < Limits.Right)
        {
            XAxis.Range.Max = Limits.Right;
            XAxis.Range.Min = Limits.Right - xSpan;
        }

        if (XAxis.Range.Min > Limits.Left)
        {
            XAxis.Range.Min = Limits.Left;
            XAxis.Range.Max = Limits.Left + xSpan;
        }

        if (YAxis.Range.Max < Limits.Top)
        {
            YAxis.Range.Max = Limits.Top;
            YAxis.Range.Min = Limits.Top - ySpan;
        }

        if (YAxis.Range.Min > Limits.Bottom)
        {
            YAxis.Range.Min = Limits.Bottom;
            YAxis.Range.Max = Limits.Bottom + ySpan;
        }
    }
}
