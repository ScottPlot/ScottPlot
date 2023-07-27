namespace ScottPlot.Rendering.RenderActions;

public class AutoAxisAnyUnsetAxes : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (IPlottable plottable in rp.Plot.PlottableList)
        {
            if (!plottable.Axes.XAxis.Range.HasBeenSet || !plottable.Axes.YAxis.Range.HasBeenSet)
            {
                rp.Plot.AutoScale(plottable.Axes.XAxis, plottable.Axes.YAxis);
            }
        }

        if (!rp.Plot.XAxis.Range.HasBeenSet) // may occur when there are no plottables with data
        {
            rp.Plot.SetAxisLimits(AxisLimits.Default);
        }
    }
}
