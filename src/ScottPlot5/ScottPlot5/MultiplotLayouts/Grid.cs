namespace ScottPlot.MultiplotLayouts;

public class Grid(int rows, int columns) : IMultiplotLayout
{
    public void ResetAllPositions(Multiplot multiplot)
    {
        double fractionPerRow = 1.0 / rows;
        double fractionPerColumn = 1.0 / columns;

        for (int i = 0; i < multiplot.Count; i++)
        {
            int rowIndex = i / columns;
            int columnIndex = i % columns;
            FractionRect fr = new(fractionPerColumn * columnIndex, fractionPerRow * rowIndex, fractionPerColumn, fractionPerRow);
            ISubplotPosition position = new SubplotPositions.Fractional(fr);
            multiplot.SetPosition(i, position);
        }
    }
}
