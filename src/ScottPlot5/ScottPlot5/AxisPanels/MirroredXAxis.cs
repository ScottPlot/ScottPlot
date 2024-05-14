namespace ScottPlot.AxisPanels;

public sealed class MirroredXAxis : XAxisBase, IXAxis
{
    private readonly Edge _edge;
    private readonly IXAxis _axis;
    public override Edge Edge => _edge;
    public override CoordinateRangeMutable Range => new(_axis.Min, _axis.Max);

    public MirroredXAxis(IXAxis axis, Edge? edge)
    {
        _axis = axis;
        _edge = edge ?? axis.Edge;
        TickGenerator = axis.TickGenerator;
    }
}
