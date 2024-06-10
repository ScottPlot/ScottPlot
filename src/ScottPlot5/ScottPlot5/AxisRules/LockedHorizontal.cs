namespace ScottPlot.AxisRules;

public class LockedHorizontal(IXAxis xAxis, double xMin, double xMax) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;
    public double XMin = xMin;
    public double XMax = xMax;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        XAxis.Range.Set(XMin, XMax);
    }
}
