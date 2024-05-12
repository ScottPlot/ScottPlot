namespace ScottPlot.AxisRules;

public class SnapToTicksY(IYAxis yAxis) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (beforeLayout)
            return;

        // do not attempt to set limits according to ticks while the window is resizing
        if (rp.Plot.RenderManager.RenderCount > 0 && rp.Plot.RenderManager.LastRender.DataRect != rp.DataRect)
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
        double newTop = newLimits.Max;
        double newBottom = newLimits.Min;

        YAxis.RegenerateTicks(new PixelLength(rp.FigureRect.Height));
        var ticks = YAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        if (ticks.Count() < 2)
            return;

        double tickDelta = ticks.Skip(1).First() - ticks.First();

        if (!topIsLocked)
        {
            var oldTop = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Max;
            var proposedNewTop = Math.Ceiling(newLimits.Max / tickDelta) * tickDelta;

            if (newLimits.Max >= oldTop || proposedNewTop < oldTop)
            { // we'll snap outwards if we can 
                newTop = proposedNewTop;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newTop = Math.Floor(newLimits.Max / tickDelta) * tickDelta;
            }
        }

        if (!bottomIsLocked)
        {
            var oldBottom = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Min;
            var proposedNewBottom = Math.Floor(newLimits.Min / tickDelta) * tickDelta;

            if (newLimits.Min <= oldBottom || proposedNewBottom > oldBottom)
            { // we'll snap outwards if we can 
                newBottom = proposedNewBottom;
            }
            else
            { // but if the snapped limit expands the range when a user requested to reduce the range we'll snap inwards
                newBottom = Math.Ceiling(newLimits.Min / tickDelta) * tickDelta;
            }
        }

        if (newTop != newBottom) YAxis.Range.Set(newBottom, newTop);
    }
}
