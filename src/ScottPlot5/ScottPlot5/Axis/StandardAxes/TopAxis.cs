namespace ScottPlot.Axis.StandardAxes;

public class TopAxis : XAxisBase, IXAxis
{
    public override Edge Edge => Edge.Top;

    public TopAxis()
    {
        TickGenerator = new TickGenerators.NumericAutomatic(false);
    }
}
