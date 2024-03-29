﻿namespace ScottPlot.AxisRules;

public class LockedVertical : IAxisRule
{
    readonly IYAxis YAxis;

    public LockedVertical(IYAxis yAxis)
    {
        YAxis = yAxis;
    }

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the last render must wait for a render to occur
        if (rp.Plot.LastRender.Count == 0)
            return;

        // TODO: reference the correct axis from the previous render
        double yMin = rp.Plot.LastRender.AxisLimits.Bottom;
        double yTop = rp.Plot.LastRender.AxisLimits.Top;
        YAxis.Range.Set(yMin, yTop);
    }
}
