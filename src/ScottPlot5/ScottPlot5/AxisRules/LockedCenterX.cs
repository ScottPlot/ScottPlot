namespace ScottPlot.AxisRules;

public class LockedCenterX(IXAxis xAxis, double xCenter) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;
    readonly double XCenter = xCenter;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        XAxis.Pan(XCenter - XAxis.Center);
    }
}
