namespace ScottPlot.MultiplotLayouts;

public class Columns : IMultiplotLayout
{
    public void ResetAllPositions(Multiplot multiplot)
    {
        double fractionPerColumn = 1.0 / multiplot.Count;
        for (int i = 0; i < multiplot.Count; i++)
        {
            FractionRect fr = new(fractionPerColumn * i, 0, fractionPerColumn, 1);
            ISubplotPosition position = new SubplotPositions.Fractional(fr);
            multiplot.SetPosition(i, position);
        }
    }
}
