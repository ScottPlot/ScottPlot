namespace ScottPlot.MultiplotLayouts;

public class Rows : IMultiplotLayout
{
    public void ResetAllPositions(Multiplot multiplot)
    {
        double fractionPerRow = 1.0 / multiplot.Count;
        for (int i = 0; i < multiplot.PositionedPlots.Count; i++)
        {
            FractionRect fr = new(0, fractionPerRow * i, 1, fractionPerRow);
            multiplot.PositionedPlots[i].Position = new SubplotPositions.Fractional(fr);
        }
    }
}
