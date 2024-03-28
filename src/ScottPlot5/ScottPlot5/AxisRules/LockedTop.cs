namespace ScottPlot.AxisRules;

public class LockedTop : IAxisRule
{
    public readonly IYAxis YAxis;
    readonly double? LockValue;

    public LockedTop(IYAxis yAxis, double? lockValue = null)
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

        double yTop = LockValue.HasValue ? (double)LockValue : rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Max;
        double yBottom = YAxis.Range.Min;
        YAxis.Range.Set(yBottom, yTop);
    }
}
