namespace ScottPlot.AxisRules;

public class LockedBottom : IAxisRule
{
    public readonly IYAxis YAxis;
    readonly double? LockValue;

    public LockedBottom(IYAxis yAxis, double? lockValue = null)
    {
        LockValue = lockValue;
        YAxis = yAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        // a locked axis won't be changed at all (vertical locking rule takes priority)
        foreach (var rule in rp.Plot.Axes.Rules)
        {
            if (rule is LockedVertical lockedVerticalRule)
            {
                if (lockedVerticalRule.YAxis == YAxis)
                {
                    // the requested axis already has a vertical lock
                    return;
                }
            }
        }

        double yBottom = LockValue.HasValue ? (double)LockValue : rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Min;
        double yTop = YAxis.Range.Max;
        YAxis.Range.Set(yBottom, yTop);
    }
}
