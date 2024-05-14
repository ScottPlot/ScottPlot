namespace ScottPlot.AxisPanels;

public class TopAxis : XAxisBase, IXAxis
{
    public override Edge Edge => Edge.Top;

    public TopAxis()
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
