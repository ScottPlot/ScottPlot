namespace ScottPlot.AxisRules;

public class SnapToTicksX(IXAxis xAxis) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (beforeLayout)
            return;

        // do not attempt to set limits according to ticks while the window is resizing
        if (rp.Plot.RenderManager.RenderCount > 0 && rp.Plot.RenderManager.LastRender.DataRect != rp.DataRect)
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
        double newRight = newLimits.Max;
        double newLeft = newLimits.Min;

        XAxis.RegenerateTicks(new PixelLength(rp.FigureRect.Width));
        var ticks = XAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        if (ticks.Count() < 2)
            return;

        double tickDelta = ticks.Skip(1).First() - ticks.First();

        if (!rightIsLocked)
        {
            var oldRight = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Max;
            var proposedNewRight = Math.Ceiling(newLimits.Max / tickDelta) * tickDelta;

            if (newLimits.Max >= oldRight || proposedNewRight < oldRight)
            { // we'll snap outwards if we can 
                newRight = proposedNewRight;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newRight = Math.Floor(newLimits.Max / tickDelta) * tickDelta;
            }
        }

        if (!leftIsLocked)
        {
            var oldLeft = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Min;
            var proposedNewLeft = Math.Floor(newLimits.Min / tickDelta) * tickDelta;

            if (newLimits.Min <= oldLeft || proposedNewLeft > oldLeft)
            { // we'll snap outwards if we can 
                newLeft = proposedNewLeft;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newLeft = Math.Ceiling(newLimits.Min / tickDelta) * tickDelta;
            }
        }
        if (newLeft != newRight) XAxis.Range.Set(newLeft, newRight);
        
    }


}
