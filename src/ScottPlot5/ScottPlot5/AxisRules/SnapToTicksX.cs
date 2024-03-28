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
            var oldRight = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Max;
            var proposedNewRight = Math.Ceiling(newLimits.Max / tickInterval) * tickInterval;

            if (newLimits.Max >= oldRight || proposedNewRight < oldRight)
            { // we'll snap outwards if we can 
                newRight = proposedNewRight;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newRight = Math.Floor(newLimits.Max / tickInterval) * tickInterval;
            }
        }

        if (!leftIsLocked)
        {
            var oldLeft = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Min;
            var proposedNewLeft = Math.Floor(newLimits.Min / tickInterval) * tickInterval;

            if (newLimits.Min <= oldLeft || proposedNewLeft > oldLeft)
            { // we'll snap outwards if we can 
                newLeft = proposedNewLeft;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newLeft = Math.Ceiling(newLimits.Min / tickInterval) * tickInterval;
            }
        }
        if (newLeft != newRight) XAxis.Range.Set(newLeft, newRight);
    }
}
