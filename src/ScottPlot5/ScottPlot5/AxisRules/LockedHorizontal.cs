namespace ScottPlot.AxisRules;

public class LockedHorizontal : IAxisRule
{
    public readonly IXAxis XAxis;

    public LockedHorizontal(IXAxis xAxis)
    {
        XAxis = xAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        double xMin = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Min;
        double xMax = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Max;
        XAxis.Range.Set(xMin, xMax);
    }
}
