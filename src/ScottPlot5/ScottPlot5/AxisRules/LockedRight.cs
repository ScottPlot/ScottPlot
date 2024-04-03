namespace ScottPlot.AxisRules;

public class LockedRight : IAxisRule
{
    public readonly IXAxis XAxis;
    readonly double? LockValue;

    public LockedRight(IXAxis xAxis, double? lockValue = null)
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

        double xRight = LockValue.HasValue ? (double)LockValue : rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Max;
        double xLeft = XAxis.Range.Min;
        XAxis.Range.Set(xLeft, xRight);
    }
}
