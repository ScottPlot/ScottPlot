namespace ScottPlot.MultiplotLayouts;

public class CustomGrid : IMultiplotLayout
{
    readonly Dictionary<Plot, GridCell> PlotCells = [];

    public PixelRect[] GetSubplotRectangles(Multiplot multiplot, PixelRect figureRect)
    {
        PixelRect[] rectangles = new PixelRect[multiplot.Count];

        Plot[] plots = multiplot.GetPlots();

        for (int i = 0; i < plots.Length; i++)
        {
            if (PlotCells.TryGetValue(plots[i], out GridCell? cell))
            {
                rectangles[i] = cell.GetRect(figureRect);
            }
            else
            {
                rectangles[i] = PixelRect.NaN;
            }
        }

        return rectangles;
    }

    public void Set(Plot plot, GridCell gridCell)
    {
        PlotCells[plot] = gridCell;
    }
}
