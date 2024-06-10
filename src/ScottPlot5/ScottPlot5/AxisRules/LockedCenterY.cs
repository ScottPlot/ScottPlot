namespace ScottPlot.AxisRules;

public class LockedCenterY(IYAxis yAxis, double yCenter) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;
    readonly double YCenter = yCenter;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        YAxis.Range.Pan(YCenter - YAxis.Range.Center);
    }
}
