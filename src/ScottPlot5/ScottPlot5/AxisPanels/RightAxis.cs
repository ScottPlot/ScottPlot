namespace ScottPlot.AxisPanels;

public class RightAxis : YAxisBase, IYAxis
{
    public override Edge Edge { get; } = Edge.Right;

    public RightAxis()
    {
        TickGenerator = new TickGenerators.NumericAutomatic();
        LabelRotation = 90;
    }
}
