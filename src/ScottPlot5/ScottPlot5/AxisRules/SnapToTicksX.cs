using System.Security.Cryptography.X509Certificates;

namespace ScottPlot.AxisRules;

public class SnapToTicksX(IXAxis xAxis) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (beforeLayout)
            return;

        var oldRight = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Max;
        var oldLeft = rp.Plot.LastRender.AxisLimitsByAxis[XAxis].Min;
        var newLimits = XAxis.Range;
        double newRight = newLimits.Max;
        double newLeft = newLimits.Min;

        // do not attempt to set limits if they have not changed
        if (newRight == oldRight & newLeft == oldLeft)
        {
            return;
        }

        // a locked axis wont be snapped (locking rules take priority)
        bool leftIsLocked = false;
        bool rightIsLocked = false;
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

        // establish which type of axis change occurred
        bool zoomedInRight = newRight < oldRight;
        bool zoomedInLeft = newLeft > oldLeft;
        bool isPanning = zoomedInLeft ^ zoomedInRight;

        // Find the ticks for the curtrent axis so we can snap to these
        XAxis.RegenerateTicks(new PixelLength(rp.DataRect.Width));
        var ticks = XAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        if (ticks.Count() < 2) return; //if there is only 1 tick we can't establish the tick interval so can't snap to a tick. 
        var tickDelta = ticks.Skip(1).First() - ticks.First();

        // As a default we'll snap outwards, then check if we should have snapped inward
        newRight = ticks.Max() + tickDelta;
        newLeft = ticks.Min() - tickDelta;

        if (zoomedInRight)
        {
            while (newRight >= oldRight)
            {
                newRight -= tickDelta;
            }
        }

        if (zoomedInLeft)
        {
            while (newLeft <= oldLeft)
            {
                newLeft += tickDelta;
            }
        }

        //This is to handle panning, which can be jumpy if we snap before it has panned more than half the tick interval
        if (isPanning & Math.Abs(newLimits.Max-oldRight) < tickDelta/2)
        {
            newRight = oldRight;
            newLeft = oldLeft;
        }

        //Now we can reset to old limits if the limits were locked by another rule
        if (rightIsLocked)
        {
            newRight = oldRight;
        }

        if (leftIsLocked)
        {
            newLeft = oldLeft;
        }

        //Now we can set the new limits that are snapped to tick intervals
        if (newLeft != newRight) XAxis.Range.Set(newLeft, newRight);

        //But, the new limits might cause a change in the tick interval! So here we will test that and update the snap if necessary
        XAxis.RegenerateTicks(new PixelLength(rp.DataRect.Width));
        ticks = XAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        var newTickDelta = ticks.Skip(1).First() - ticks.First();

        if (newTickDelta != tickDelta)
        {//tick interval has changed

            if (ticks.Max() != newRight & !rightIsLocked)
            {// right limit is no longer on a tick because the tick interval has changed
                newRight = zoomedInRight ? ticks.Max() : ticks.Max() + newTickDelta;
            }

            if (ticks.Min() != newLeft & !leftIsLocked)
            {// right limit is no longer on a tick because the tick interval has changed
                newLeft = zoomedInLeft ? ticks.Min() : ticks.Min() - newTickDelta;
            }

            // Finally, we need to reset the limits
            if (newLeft != newRight) XAxis.Range.Set(newLeft, newRight);
        }
    }

}
