namespace ScottPlot.MultiplotLayouts;

public class Grid(int rows, int columns) : IMultiplotLayout
{
    public PixelRect[] GetSubplotRectangles(SubplotCollection subplots, PixelRect figureRect)
    {
        PixelRect[] rectangles = new PixelRect[subplots.Count];

        double fractionPerRow = 1.0 / rows;
        double fractionPerColumn = 1.0 / columns;

        for (int i = 0; i < subplots.Count; i++)
        {
            int rowIndex = i / columns;
            int columnIndex = i % columns;
            FractionRect fr = new(fractionPerColumn * columnIndex, fractionPerRow * rowIndex, fractionPerColumn, fractionPerRow);
            rectangles[i] = fr.GetPixelRect(figureRect);
        }

        return rectangles;
    }
}
