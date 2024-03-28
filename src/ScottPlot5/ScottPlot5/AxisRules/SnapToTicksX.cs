namespace ScottPlot.AxisRules;

public class SnapToTicksX : IAxisRule
{
    public readonly IXAxis XAxis;

    public SnapToTicksX(IXAxis xAxis)
    {
        XAxis = xAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        bool leftIsLocked = false;
        bool rightIsLocked = false;
        // a locked axis wont be snapped (locking rules take priority)
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

            if (rule is LockedLeft lockedLeftRule)
            {
                if (lockedLeftRule.XAxis == XAxis)
                {
                    // the requested axis already has a left lock
                    leftIsLocked = true;
                }
            }

            if (rule is LockedRight lockedRightRule)
            {
                if (lockedRightRule.XAxis == XAxis)
                {
                    // the requested axis already has a right lock
                    rightIsLocked = true;
                }
            }
        }


        var newLimits = XAxis.Range;
        XAxis.RegenerateTicks(rp.FigureRect.Width);
        var ticks = XAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).ToList();
        //var ticks = rp.Plot.Axes.Bottom.TickGenerator.Ticks.Where(tick => tick.IsMajor).ToList();

        if (ticks.Count < 2) return;
        var tickInterval = ticks[1].Position - ticks[0].Position;

        double newRight = newLimits.Max;
        double newLeft = newLimits.Min;

        if (!rightIsLocked)
        {
            if (newLimits.Max >= rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Max)
            {
                newRight = Math.Ceiling(newLimits.Max / tickInterval) * tickInterval;
            }
            else
            {
                newRight = Math.Floor(newLimits.Max / tickInterval) * tickInterval;
            }
        }

        if (!leftIsLocked)
        {
            if (newLimits.Min <= rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Min)
            {
                newLeft = Math.Floor(newLimits.Min / tickInterval) * tickInterval;
            }
            else
            {
                newLeft = Math.Ceiling(newLimits.Min / tickInterval) * tickInterval;
            }
        }
        if(newLeft!=newRight) XAxis.Range.Set(newLeft, newRight);
    }
}
