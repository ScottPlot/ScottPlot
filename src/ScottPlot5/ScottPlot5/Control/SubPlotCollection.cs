namespace ScottPlot.Control;

public class SubPlotCollection
{
    private readonly List<SubPlot> SubPlots = new();

    public Plot Add()
    {
        Plot plot = new();
        plot.RenderManager.ClearCanvasBeforeRendering = !SubPlots.Any();
        SubPlot sp = new(plot, new PixelRect());
        SubPlots.Add(sp);
        return plot;
    }

    public void LayoutHorizontally(PixelRect figureRect)
    {
        float width = figureRect.Width / SubPlots.Count;

        for (int i = 0; i < SubPlots.Count; i++)
        {
            float left = figureRect.Left + i * width;
            float right = figureRect.Left + left + width;
            float bottom = figureRect.Bottom;
            float top = figureRect.Top;
            SubPlots[i].Rect = new(left, right, bottom, top);
        }
    }

    public void Render(SKCanvas canvas, PixelRect rect)
    {
        LayoutHorizontally(rect);

        foreach (var subPlot in SubPlots)
        {
            subPlot.Plot.Render(canvas, subPlot.Rect);
        }
    }
}
