namespace ScottPlot.AxisRules;

public class MaximumBoundary : IAxisRule
{
    readonly IXAxis XAxis;
    readonly IYAxis YAxis;
    public AxisLimits Limits { get; set; }

    public MaximumBoundary(IXAxis xAxis, IYAxis yAxis, AxisLimits limits)
    {
        XAxis = xAxis;
        YAxis = yAxis;
        Limits = limits;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        double xSpan = Math.Min(XAxis.Range.Span, Limits.HorizontalSpan);
        double ySpan = Math.Min(YAxis.Range.Span, Limits.VerticalSpan);

        if (XAxis.Range.Max > Limits.Right)
        {
            XAxis.Range.Max = Limits.Right;
            XAxis.Range.Min = Limits.Right - xSpan;
        }

        if (XAxis.Range.Min < Limits.Left)
        {
            XAxis.Range.Min = Limits.Left;
            XAxis.Range.Max = Limits.Left + xSpan;
        }

        if (YAxis.Range.Max > Limits.Top)
        {
            YAxis.Range.Max = Limits.Top;
            YAxis.Range.Min = Limits.Top - ySpan;
        }

        if (YAxis.Range.Min < Limits.Bottom)
        {
            YAxis.Range.Min = Limits.Bottom;
            YAxis.Range.Max = Limits.Bottom + ySpan;
        }
    }
}
