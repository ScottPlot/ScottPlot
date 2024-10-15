namespace ScottPlot.AxisRules;

public class LockedRight(IXAxis xAxis, double xMax) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;
    readonly double XMax = xMax;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        XAxis.Range.Value2 = XMax;
    }
}
