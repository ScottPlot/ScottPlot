namespace ScottPlot.Rendering.RenderActions;

public class RegenerateTicks : IRenderAction
{
    public void Render(RenderPack rp)
    {
        var xAxesWithData = rp.Plot.PlottableList.Select(x => x.Axes.XAxis).Distinct();
        foreach (IXAxis xAxis in xAxesWithData)
        {
            xAxis.RegenerateTicks(rp.DataRect.Width);
        }

        var yAxesWithData = rp.Plot.PlottableList.Select(x => x.Axes.YAxis).Distinct();
        foreach (IYAxis yAxis in yAxesWithData)
        {
            yAxis.RegenerateTicks(rp.DataRect.Height);
        }
    }
}
