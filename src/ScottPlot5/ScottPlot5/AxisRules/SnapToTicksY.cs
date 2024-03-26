namespace ScottPlot.AxisRules;

public class SnapToTicksY : IAxisRule
{
    readonly IYAxis YAxis;

    public SnapToTicksY(IYAxis yAxis)
    {
        YAxis = yAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        bool topIsLocked = false;
        bool bottomIsLocked = false;
        // a locked axis wont be snapped (locking rules take priority)
        foreach (var rule in rp.Plot.Axes.Rules)
        {
            if (rule is LockedVertical lockedVerticalRule)
            {
                if(lockedVerticalRule.YAxis == YAxis)
                {
                    // the requested axis already has a vertical lock
                    return;
                }
            }

            if (rule is LockedTop lockedTopRule)
            {
                if (lockedTopRule.YAxis == YAxis)
                {
                    // the requested axis already has a top lock
                    topIsLocked = true;
                }
            }

            if (rule is LockedBottom lockedBottomRule)
            {
                if (lockedBottomRule.YAxis == YAxis)
                {
                    // the requested axis already has a bottom lock
                    bottomIsLocked = true;
                }
            }
        }

        var newLimits = YAxis.Range;
        var ticks = YAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).ToList();
        //var ticks = rp.Plot.Axes.Bottom.TickGenerator.Ticks.Where(tick => tick.IsMajor).ToList();
        
        if (ticks.Count < 2) return;
        var tickInterval = ticks[1].Position - ticks[0].Position;

        double newTop = newLimits.Max;
        double newBottom = newLimits.Min;

        if (!topIsLocked)
        {
            if (newLimits.Max >= rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Max)
            {
                newTop = Math.Ceiling(newLimits.Max / tickInterval) * tickInterval;
            }
            else
            {
                newTop = Math.Floor(newLimits.Max / tickInterval) * tickInterval;
            }
        }

        if (!bottomIsLocked)
        {
            if (newLimits.Min <= rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Min)
            {
                newBottom = Math.Floor(newLimits.Min / tickInterval) * tickInterval;
            }
            else
            {
                newBottom = Math.Ceiling(newLimits.Min / tickInterval) * tickInterval;
            }
        }

        YAxis.Range.Set(newBottom, newTop);
    }
}
