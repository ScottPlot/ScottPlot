namespace ScottPlot.AxisRules;

public class MinimumBoundary : IAxisRule
{
    readonly IXAxis XAxis;
    readonly IYAxis YAxis;
    public AxisLimits Limits { get; set; }

    public MinimumBoundary(IXAxis xAxis, IYAxis yAxis, AxisLimits limits)
    {
        XAxis = xAxis;
        YAxis = yAxis;
        Limits = limits;
    }

    public void Apply(RenderPack rp)
    {
        XAxis.Range.Min = Math.Min(XAxis.Range.Min, Limits.Left);
        XAxis.Range.Max = Math.Max(XAxis.Range.Max, Limits.Right);
        YAxis.Range.Min = Math.Min(YAxis.Range.Min, Limits.Bottom);
        YAxis.Range.Max = Math.Max(YAxis.Range.Max, Limits.Top);
    }
}
