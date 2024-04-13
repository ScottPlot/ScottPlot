namespace ScottPlot.AxisPanels;

public class BottomAxis : XAxisBase, IXAxis
{
    public override Edge Edge => Edge.Bottom;

    public BottomAxis()
    {
        TickGenerator = new TickGenerators.NumericAutomatic();
    }
}
