namespace ScottPlot.Rendering.RenderActions;

public class RenderPlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (var plottable in rp.Plot.PlottableList.Where(x => x.IsVisible))
        {
            plottable.Axes.DataRect = rp.DataRect;
            rp.Canvas.Save();

            if (plottable is IPlottableGL plottableGL)
            {
                plottableGL.Render(rp);
            }
            else
            {
                rp.Canvas.ClipRect(rp.DataRect.ToSKRect());
                plottable.Render(rp);
            }

            rp.Canvas.Restore();
        }
    }
}
