namespace ScottPlot.Rendering.RenderActions;

public class AutoScaleUnsetAxes : IRenderAction
{
    public void Render(RenderPack rp)
    {
        IEnumerable<IPlottable> visiblePlottables = rp.Plot.PlottableList.Where(x => x.IsVisible);

        if (!visiblePlottables.Any())
        {
            if (!rp.Plot.BottomAxis.Range.HasBeenSet)
            {
                rp.Plot.SetAxisLimitsX(AxisLimits.Default);
            }

            if (!rp.Plot.LeftAxis.Range.HasBeenSet)
            {
                rp.Plot.SetAxisLimitsY(AxisLimits.Default);
            }

            return;
        }

        foreach (IPlottable plottable in rp.Plot.PlottableList)
        {
            bool scaleX = !plottable.Axes.XAxis.Range.HasBeenSet;
            bool scaleY = !plottable.Axes.YAxis.Range.HasBeenSet;
            rp.Plot.AutoScale(plottable.Axes.XAxis, plottable.Axes.YAxis, scaleX, scaleY);
        }
    }
}
