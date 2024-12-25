namespace ScottPlot.MultiplotLayouts;

public class Rows : IMultiplotLayout
{
    public void ResetAllPositions(Multiplot multiplot)
    {
        double fractionPerRow = 1.0 / multiplot.Count;
        for (int i = 0; i < multiplot.Count; i++)
        {
            FractionRect fr = new(0, fractionPerRow * i, 1, fractionPerRow);
            ISubplotPosition position = new SubplotPositions.Fractional(fr);
            multiplot.SetPosition(i, position);
        }
    }
}
