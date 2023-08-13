namespace ScottPlot.MultiplotLayouts;

public class TileHorizontally : IMultiplotLayout
{
    public List<PositionedPlot> GetPositionedPlots(List<Plot> plots, PixelRect figureRect)
    {
        List<PositionedPlot> subPlots = new(plots.Count);

        float width = figureRect.Width / plots.Count;

        for (int i = 0; i < plots.Count; i++)
        {
            float left = figureRect.Left + i * width;
            float right = figureRect.Left + left + width;
            float bottom = figureRect.Bottom;
            float top = figureRect.Top;
            PixelRect rect = new(left, right, bottom, top);
            PositionedPlot pp = new(plots[i], rect);
            subPlots.Add(pp);
        }

        return subPlots;
    }
}
