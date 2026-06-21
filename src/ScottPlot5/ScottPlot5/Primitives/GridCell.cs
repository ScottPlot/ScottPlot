namespace ScottPlot;

/// <summary>
/// Represents a single cell in a rectangular grid.
/// </summary>
public class GridCell
{
    private readonly FractionRect FractionRect;

    public GridCell(int rowIndex, int colIndex, int rowCount, int colCount, int rowSpan = 1, int colSpan = 1)
    {
        double unitWidth = 1.0 / colCount;
        double unitHeight = 1.0 / rowCount;
        double xOffset = unitWidth * colIndex;
        double yOffset = unitHeight * rowIndex;
        FractionRect = new(xOffset, yOffset, unitWidth * colSpan, unitHeight * rowSpan);
    }

    /// <summary>
    /// Return the rectangle for this cell given the dimensions of the full grid
    /// </summary>
    public PixelRect GetRect(PixelRect figureRect)
    {
        return FractionRect.GetPixelRect(figureRect.Width, figureRect.Height);
    }
}
