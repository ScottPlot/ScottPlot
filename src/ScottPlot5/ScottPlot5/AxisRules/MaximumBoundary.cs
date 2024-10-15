namespace ScottPlot.AxisRules;

public class MaximumBoundary(IXAxis xAxis, IYAxis yAxis, AxisLimits limits) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;
    public AxisLimits Limits { get; set; } = limits;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        double xSpan = Math.Min(XAxis.Range.Span, Limits.HorizontalSpan);
        double ySpan = Math.Min(YAxis.Range.Span, Limits.VerticalSpan);

        if (XAxis.Range.Value2 > Limits.Right)
        {
            XAxis.Range.Value2 = Limits.Right;
            XAxis.Range.Value1 = Limits.Right - xSpan;
        }

        if (XAxis.Range.Value1 < Limits.Left)
        {
            XAxis.Range.Value1 = Limits.Left;
            XAxis.Range.Value2 = Limits.Left + xSpan;
        }

        if (YAxis.Range.Value2 > Limits.Top)
        {
            YAxis.Range.Value2 = Limits.Top;
            YAxis.Range.Value1 = Limits.Top - ySpan;
        }

        if (YAxis.Range.Value1 < Limits.Bottom)
        {
            YAxis.Range.Value1 = Limits.Bottom;
            YAxis.Range.Value2 = Limits.Bottom + ySpan;
        }
    }
}
