namespace ScottPlot.AxisRules;

public class LockedHorizontal : IAxisRule
{
    readonly IXAxis XAxis;

    public LockedHorizontal(IXAxis xAxis)
    {
        XAxis = xAxis;
    }

    public void Apply(RenderPack rp)
    {
        // TODO: reference the correct axis from the previous render
        double xMin = rp.Plot.LastRender.AxisLimits.Left;
        double xMax = rp.Plot.LastRender.AxisLimits.Right;
        XAxis.Range.Set(xMin, xMax);
    }
}
