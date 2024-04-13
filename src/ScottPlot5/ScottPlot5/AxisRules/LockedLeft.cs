namespace ScottPlot.AxisRules;

public class LockedLeft : IAxisRule
{
    public readonly IXAxis XAxis;
    readonly double? LockValue;

    public LockedLeft(IXAxis xAxis, double? lockValue = null)
    {
        LockValue = lockValue;
        XAxis = xAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        // a locked axis won't be changed at all (horizontal locking rule takes priority)
        foreach (var rule in rp.Plot.Axes.Rules)
        {
            if (rule is LockedHorizontal lockedHorizontalRule)
            {
                if (lockedHorizontalRule.XAxis == XAxis)
                {
                    // the requested axis already has a horizontal lock
                    return;
                }
            }
        }

        double xLeft = LockValue.HasValue ? (double)LockValue : rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Min;
        double xRight = XAxis.Range.Max;
        XAxis.Range.Set(xLeft, xRight);
    }
}
