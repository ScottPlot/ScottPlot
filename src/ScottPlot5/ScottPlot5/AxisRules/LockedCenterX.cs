namespace ScottPlot.AxisRules;

public class LockedCenterX : IAxisRule
{
    public readonly IXAxis XAxis;
    readonly double? LockValue;

    public LockedCenterX(IXAxis xAxis, double? lockValue = null)
    {
        LockValue = lockValue;
        XAxis = xAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        // a locked axis won't be changed at all (horizontal locking rule takes priority)
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
        }

        var oldAxisRange = rp.Plot.LastRender.AxisLimitsByAxis[XAxis];

        // if axis is also locked on left or right X it shouldn't change at all.
        foreach (var rule in rp.Plot.Axes.Rules)
        {
            if (rule is LockedRight lockedRightRule)
            {
                if (lockedRightRule.XAxis == XAxis)
                {
                    XAxis.Range.Set(oldAxisRange.Min, oldAxisRange.Max);
                    return;
                }
            }

            if (rule is LockedLeft lockedLeftRule)
            {
                if (lockedLeftRule.XAxis == XAxis)
                {
                    XAxis.Range.Set(oldAxisRange.Min, oldAxisRange.Max);
                    return;
                }
            }
        }

        double xCenter = LockValue.HasValue ? (double)LockValue : oldAxisRange.Center;
        bool XAxisInverted = XAxis.Range.Min > XAxis.Range.Max;
        double spanDelta = XAxis.Range.Span - oldAxisRange.Span;

        // first we will adjust the span symetrically
        double xLeft = XAxisInverted ? oldAxisRange.Min + spanDelta / 2 : oldAxisRange.Min - spanDelta / 2;
        double xRight = XAxisInverted ? oldAxisRange.Max - spanDelta / 2 : oldAxisRange.Max + spanDelta / 2;

        // now we will recenter
        double currentCenter = xLeft / 2 + xRight / 2;
        xLeft += (xCenter - currentCenter);
        xRight += (xCenter - currentCenter);

        XAxis.Range.Set(xLeft, xRight);
    }
}
