namespace ScottPlot.MultiplotLayouts;

public class Grid(int rows, int columns) : IMultiplotLayout
{
    public void ResetAllPositions(Multiplot multiplot)
    {
        double fractionPerRow = 1.0 / rows;
        double fractionPerColumn = 1.0 / columns;

        for (int i = 0; i < multiplot.PositionedPlots.Count; i++)
        {
            int rowIndex = columns / i;
            int columnIndex = i % columns;
            FractionRect fr = new(fractionPerColumn * i, fractionPerRow * i, fractionPerColumn, fractionPerRow);
            multiplot.PositionedPlots[i].Position = new SubplotPositions.Fractional(fr);
        }
    }
}
