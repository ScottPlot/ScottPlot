namespace ScottPlot.AxisRules;

public class LockedTop(IYAxis yAxis, double yMax) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;
    readonly double YMax = yMax;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        YAxis.Range.Max = YMax;
    }
}
