namespace ScottPlot.AxisPanels;

public class TopAxis : XAxisBase, IXAxis
{
    public override Edge Edge => Edge.Top;

    public TopAxis()
    {
        TickGenerator = new TickGenerators.NumericAutomatic();
    }
}
