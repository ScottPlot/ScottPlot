namespace ScottPlot.AxisRules;

public class LockedBottom(IYAxis yAxis, double yMin) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;
    readonly double YMin = yMin;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        YAxis.Range.Min = YMin;
    }
}
