namespace ScottPlot.AxisRules;

public class LockedCenterY : IAxisRule
{
    public readonly IYAxis YAxis;
    readonly double? LockValue;

    public LockedCenterY(IYAxis yAxis, double? lockValue = null)
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

        var oldAxisRange = rp.Plot.LastRender.AxisLimitsByAxis[YAxis];

        // if axis is also locked on top or bottom X it shouldn't change at all.
        foreach (var rule in rp.Plot.Axes.Rules)
        {
            if (rule is LockedTop lockedTopRule)
            {
                if (lockedTopRule.YAxis == YAxis)
                {
                    YAxis.Range.Set(oldAxisRange.Min, oldAxisRange.Max);
                    return;
                }
            }

            if (rule is LockedBottom lockedBottomRule)
            {
                if (lockedBottomRule.YAxis == YAxis)
                {
                    YAxis.Range.Set(oldAxisRange.Min, oldAxisRange.Max);
                    return;
                }
            }
        }

        double yCenter = LockValue.HasValue ? (double)LockValue : oldAxisRange.Center;
        bool YAxisInverted = YAxis.Range.Min > YAxis.Range.Max;
        double spanDelta = YAxis.Range.Span - oldAxisRange.Span;

        // first we will adjust the span symetrically
        double yBottom = YAxisInverted ? oldAxisRange.Min + spanDelta / 2 : oldAxisRange.Min - spanDelta / 2;
        double yTop = YAxisInverted ? oldAxisRange.Max - spanDelta / 2 : oldAxisRange.Max + spanDelta / 2;

        // now we will recenter
        double currentCenter = yBottom / 2 + yTop / 2;
        yBottom += (yCenter - currentCenter);
        yTop += (yCenter - currentCenter);

        YAxis.Range.Set(yBottom, yTop);
    }
}
