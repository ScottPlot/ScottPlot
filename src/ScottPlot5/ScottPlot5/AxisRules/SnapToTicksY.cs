namespace ScottPlot.AxisRules;

public class SnapToTicksY(IYAxis yAxis) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (beforeLayout)
            return;
        if (rp.Plot.LastRender.Count == 0)
            return;

        var inverted = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].IsInverted;
        var oldTop = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Max;
        var oldBottom = rp.Plot.LastRender.AxisLimitsByAxis[YAxis].Min;
        var newLimits = YAxis.Range;
        double newTop = newLimits.Max;
        double newBottom = newLimits.Min;

        // do not attempt to set limits if they have not changed
        if (newTop == oldTop & newBottom == oldBottom)
        {
            return;
        }

        // a locked axis wont be snapped (locking rules take priority)
        bool topIsLocked = false;
        bool bottomIsLocked = false;
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

        // establish which type of axis change occurred
        bool zoomedInTop = Math.Abs(newTop - oldBottom) < Math.Abs(oldTop - oldBottom);
        bool zoomedInBottom = Math.Abs(newBottom - oldTop) < Math.Abs(oldBottom - oldTop);
        bool isPanning = (zoomedInBottom ^ zoomedInTop) & (newTop != oldTop) & (newBottom != oldBottom);


        YAxis.RegenerateTicks(new PixelLength(rp.DataRect.Height));
        var ticks = YAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        if (ticks.Count() < 2) return; //if there is only 1 tick we can't establish the tick interval so can't snap to a tick. 
        double tickDelta = ticks.Skip(1).First() - ticks.First();

        // As a default we'll snap outwards, then check if we should have snapped inward
        newTop = inverted ? ticks.Min() - tickDelta : ticks.Max() + tickDelta;
        newBottom = inverted ? ticks.Max() + tickDelta : ticks.Min() - tickDelta;

        if (zoomedInTop)
        {
            while (Math.Abs(newTop - oldBottom) >= Math.Abs(oldTop - oldBottom))
            {
                newTop += inverted ? tickDelta : -tickDelta;
            }
        }

        if (zoomedInBottom)
        {
            while (Math.Abs(newBottom - oldTop) >= Math.Abs(oldBottom - oldTop))
            {
                newBottom += inverted ? -tickDelta : tickDelta;
            }
        }

        //This is to handle panning, which can be jumpy if we snap before it has panned more than half the tick interval
        if (isPanning & Math.Abs(newLimits.Max - oldTop) < tickDelta / 2)
        {
            newTop = oldTop;
            newBottom = oldBottom;
        }

        //Now we can reset to old limits if the limits were locked by another rule
        if (topIsLocked)
        {
            newTop = oldTop;
        }

        if (bottomIsLocked)
        {
            newBottom = oldBottom;
        }

        //Now we can set the new limits that are snapped to tick intervals
        if (newTop != newBottom) YAxis.Range.Set(newBottom, newTop);

        //But, the new limits might cause a change in the tick interval! So here we will test that and update the snap if necessary
        YAxis.RegenerateTicks(new PixelLength(rp.DataRect.Height));
        ticks = YAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        var newTickDelta = ticks.Skip(1).First() - ticks.First();

        if (newTickDelta != tickDelta)
        {//tick interval has changed

            if (newTop != (inverted ? ticks.Min() : ticks.Max()) & !topIsLocked)
            {// Top limit is no longer on a tick because the tick interval has changed
                if (zoomedInTop)
                {
                    newTop = inverted ? ticks.Min() : ticks.Max();
                }
                else
                {
                    newTop = inverted ? ticks.Min() - newTickDelta : ticks.Max() + newTickDelta;
                }
            }

            if (newBottom != (inverted ? ticks.Max() : ticks.Min()) & !bottomIsLocked)
            {// Top limit is no longer on a tick because the tick interval has changed
                if (zoomedInBottom)
                {
                    newBottom = inverted ? ticks.Max() : ticks.Min();
                }
                else
                {
                    newBottom = inverted ? ticks.Max() + newTickDelta : ticks.Min() - newTickDelta;
                }
            }

            // Finally, we need to reset the limits
            if (newBottom != newTop) YAxis.Range.Set(newBottom, newTop);
        }

    }
}
