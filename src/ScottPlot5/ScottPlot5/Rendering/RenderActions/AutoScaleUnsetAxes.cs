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
            plot.AutoScale(
                xAxis: plottable.Axes.XAxis,
                yAxis: plottable.Axes.YAxis,
                horizontal: !plottable.Axes.XAxis.Range.HasBeenSet,
                vertical: !plottable.Axes.YAxis.Range.HasBeenSet);
        }
    }

    private void ApplyDefaultLimitsToUnsetAxes(Plot plot)
    {
        if (!plot.BottomAxis.Range.HasBeenSet)
        {
            plot.SetAxisLimitsX(AxisLimits.Default);
        }

        if (!plot.LeftAxis.Range.HasBeenSet)
        {
            plot.SetAxisLimitsY(AxisLimits.Default);
        }
    }
}
