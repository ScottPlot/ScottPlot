namespace ScottPlot.MultiplotLayouts;

public class Columns : IMultiplotLayout
{
    public void ResetAllPositions(Multiplot multiplot)
    {
        double fractionPerColumn = 1.0 / multiplot.Count;
        for (int i = 0; i < multiplot.PositionedPlots.Count; i++)
        {
            FractionRect fr = new(fractionPerColumn * i, 0, fractionPerColumn, 1);
            multiplot.PositionedPlots[i].Position = new SubplotPositions.Fractional(fr);
        }
    }
}
