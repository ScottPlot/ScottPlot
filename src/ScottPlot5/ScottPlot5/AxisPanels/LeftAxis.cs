namespace ScottPlot.AxisPanels;

public class LeftAxis : YAxisBase, IYAxis
{
    public override Edge Edge { get; } = Edge.Left;

    public LeftAxis()
    {
        TickGenerator = new TickGenerators.NumericAutomatic();
        LabelRotation = -90;
    }
}
