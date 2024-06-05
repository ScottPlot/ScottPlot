namespace ScottPlot.AxisRules;

public class LockedLeft(IXAxis xAxis, double xMin) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;
    readonly double XMin = xMin;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        XAxis.Range.Min = XMin;
    }
}
