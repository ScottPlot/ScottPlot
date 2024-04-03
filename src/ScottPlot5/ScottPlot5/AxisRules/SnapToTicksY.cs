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
                if (lockedVerticalRule.YAxis == YAxis)
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
        YAxis.RegenerateTicks(rp.FigureRect.Height);
        var ticks = YAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).ToList();
        //var ticks = rp.Plot.Axes.Bottom.TickGenerator.Ticks.Where(tick => tick.IsMajor).ToList();

        if (ticks.Count < 2) return;
        var tickInterval = ticks[1].Position - ticks[0].Position;

        double newTop = newLimits.Max;
        double newBottom = newLimits.Min;

        if (!topIsLocked)
        {
            var oldTop = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Max;
            var proposedNewTop = Math.Ceiling(newLimits.Max / tickInterval) * tickInterval;

            if (newLimits.Max >= oldTop || proposedNewTop < oldTop)
            { // we'll snap outwards if we can 
                newTop = proposedNewTop;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newTop = Math.Floor(newLimits.Max / tickInterval) * tickInterval;
            }
        }

        if (!bottomIsLocked)
        {
            var oldBottom = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Min;
            var proposedNewBottom = Math.Floor(newLimits.Min / tickInterval) * tickInterval;

            if (newLimits.Min <= oldBottom || proposedNewBottom > oldBottom)
            { // we'll snap outwards if we can 
                newBottom = proposedNewBottom;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newBottom = Math.Ceiling(newLimits.Min / tickInterval) * tickInterval;
            }
        }

        if (newTop != newBottom) YAxis.Range.Set(newBottom, newTop);
    }
}
