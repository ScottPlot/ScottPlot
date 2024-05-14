namespace ScottPlot.AxisPanels;

public class BottomAxis : XAxisBase, IXAxis
{
    public override Edge Edge => Edge.Bottom;

    public BottomAxis()
    {
        TickGenerator = new TickGenerators.NumericAutomatic();
    }

    public void SetTickets(double[] xs, string[] labels)
    {
        TickGenerators.NumericManual gen = new();
        gen.SetTickets(xs, labels);
        TickGenerator = gen;
    }
}
