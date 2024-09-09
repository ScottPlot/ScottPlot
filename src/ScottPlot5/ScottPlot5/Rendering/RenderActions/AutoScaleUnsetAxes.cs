namespace ScottPlot.Rendering.RenderActions;

public class AutoScaleUnsetAxes : IRenderAction
{
    public void Render(RenderPack rp)
    {
        IEnumerable<IPlottable> visiblePlottables = rp.Plot.PlottableList.Where(x => x.IsVisible);

        if (visiblePlottables.Any())
        {
            AutoscaleUnsetAxesToData(rp.Plot);
        }
        else
        {
            ApplyDefaultLimitsToUnsetAxes(rp.Plot);
        }
    }

    private void AutoscaleUnsetAxesToData(Plot plot)
    {
        foreach (IPlottable plottable in plot.PlottableList)
        {
            bool xAxisNeedsScaling = !plottable.Axes.XAxis.HasBeenSet;
            bool yAxisNeedsScaling = !plottable.Axes.YAxis.HasBeenSet;
            if (xAxisNeedsScaling || yAxisNeedsScaling)
            {
                plot.Axes.AutoScale(
                    xAxis: plottable.Axes.XAxis,
                    yAxis: plottable.Axes.YAxis,
                    horizontal: xAxisNeedsScaling,
                    vertical: yAxisNeedsScaling);
            }
        }
    }

    private void ApplyDefaultLimitsToUnsetAxes(Plot plot)
    {
        if (!plot.Axes.Bottom.HasBeenSet)
        {
            plot.Axes.SetLimitsX(AxisLimits.Default);
        }

        if (!plot.Axes.Left.HasBeenSet)
        {
            plot.Axes.SetLimitsY(AxisLimits.Default);
        }
    }
}
