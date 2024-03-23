namespace ScottPlot.Rendering.RenderActions;

public class RenderPlottables : IRenderAction
{
    public void Render(RenderPack rp)
    {
        foreach (IPlottable plottable in rp.Plot.PlottableList)
        {
            if (!plottable.IsVisible)
                continue;

            plottable.Axes.DataRect = rp.DataRect;
            SKCanvas canvas = rp.Canvas;
            canvas.Save();


            if (plottable is IPlottableGL plottableGL)
            {
                plottableGL.Render(rp);
            }
            else
            {
                rp.ClipToDataArea();
                plottable.Render(rp);
            }

            canvas.Restore();
        }
    }
}
