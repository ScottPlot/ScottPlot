namespace ScottPlot.SubplotPositions;

public class GridCell : ISubplotPosition
{
    public FractionRect FractionRect { get; }

    public GridCell(int rowIndex, int colIndex, int rowCount, int colCount)
    {
        double width = 1.0 / colCount;
        double height = 1.0 / rowCount;
        double xOffset = width * colIndex;
        double yOffset = height * rowIndex;
        FractionRect = new(xOffset, yOffset, width, height);
    }

    public PixelRect GetRect(PixelRect figureRect)
    {
        return FractionRect.GetPixelRect(figureRect.Width, figureRect.Height);
    }
}
