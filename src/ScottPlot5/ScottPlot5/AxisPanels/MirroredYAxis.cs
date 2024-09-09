namespace ScottPlot.AxisPanels;

public sealed class MirroredYAxis : YAxisBase, IYAxis
{
    private readonly Edge _edge;
    private readonly IYAxis _axis;
    public override Edge Edge => _edge;

    public MirroredYAxis(IYAxis axis, Edge? edge)
    {
        _axis = axis;
        _edge = edge ?? axis.Edge;
        TickGenerator = axis.TickGenerator;
    }
}
