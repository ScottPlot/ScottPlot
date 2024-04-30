namespace ScottPlot.Rendering.RenderActions;

public class RenderPlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        IPlottable[] visiblePlottables = rp.Plot.PlottableList.Where(x => x.IsVisible).ToArray();

        foreach (IPlottable plottable in visiblePlottables)
        {
            plottable.Axes.DataRect = rp.DataRect;

            rp.CanvasState.Save();

            if (plottable is IPlottableGL plottableGL)
            {
                plottableGL.Render(rp);
            }
            else
            {
                rp.CanvasState.Clip(rp.DataRect);
                plottable.Render(rp);
            }

            rp.CanvasState.DisableClipping();
        }
    }
}
