namespace ScottPlot.AxisRules;

public class LockedHorizontal : IAxisRule
{
    readonly IXAxis XAxis;

    public LockedHorizontal(IXAxis xAxis)
    {
        XAxis = xAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        // TODO: reference the correct axis from the previous render
        double xMin = rp.Plot.LastRender.AxisLimits.Left;
        double xMax = rp.Plot.LastRender.AxisLimits.Right;
        XAxis.Range.Set(xMin, xMax);
    }
}
