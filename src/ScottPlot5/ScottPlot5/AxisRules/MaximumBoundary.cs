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

    public void Apply(RenderPack rp)
    {
        XAxis.Range.Min = Math.Max(XAxis.Range.Min, Limits.Left);
        XAxis.Range.Max = Math.Min(XAxis.Range.Max, Limits.Right);
        YAxis.Range.Min = Math.Max(YAxis.Range.Min, Limits.Bottom);
        YAxis.Range.Max = Math.Min(YAxis.Range.Max, Limits.Top);
    }
}
