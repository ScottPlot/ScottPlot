namespace ScottPlot.AxisRules;

public class LockedVertical : IAxisRule
{
    public readonly IYAxis YAxis;

    public LockedVertical(IYAxis yAxis)
    {
        YAxis = yAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        double yMin = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Min;
        double yTop = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Max;
        YAxis.Range.Set(yMin, yTop);
    }
}
