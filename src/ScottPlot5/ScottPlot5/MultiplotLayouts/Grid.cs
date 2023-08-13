namespace ScottPlot.MultiplotLayouts;

public class Grid : IMultiplotLayout
{
    private readonly int Rows;
    private readonly int Columns;

    public Grid(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
    }

    public List<PositionedPlot> GetPositionedPlots(List<Plot> plots, PixelRect figureRect)
    {
        List<PositionedPlot> subPlots = new(plots.Count);

        float width = figureRect.Width / Columns;
        float height = figureRect.Height / Rows;

        for (int i = 0; i < plots.Count; i++)
        {
            int column = i % Columns;
            int row = i / Columns;
            float xOffset = column * width;
            float yOffset = row * height;
            PixelRect rect = new(xOffset, xOffset + width, yOffset + height, yOffset);
            PositionedPlot pp = new(plots[i], rect);
            subPlots.Add(pp);
        }

        return subPlots;
    }
}
