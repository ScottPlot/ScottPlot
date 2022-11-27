
namespace ScottPlot.Axis.StandardAxes;
public sealed class MirroredXAxis : XAxisBase, IXAxis
{
    private readonly Edge _edge;
    private readonly IXAxis _axis;
    public override Edge Edge => _edge;
    public override CoordinateRange Range => new(_axis.Left, _axis.Right);

    public MirroredXAxis(IXAxis axis, Edge? edge)
    {
        _axis = axis;
        _edge = edge ?? axis.Edge;
        TickGenerator = axis.TickGenerator;
    }
}
