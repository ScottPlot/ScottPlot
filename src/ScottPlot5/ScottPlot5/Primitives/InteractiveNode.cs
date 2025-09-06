using ScottPlot.Interfaces;

namespace ScottPlot;

public class InteractiveNode(IInteractivePlottable parent, int index)
{
    public IInteractivePlottable Parent { get; } = parent;
    public int NodeIndex { get; } = index;
    public override string ToString() => $"{Parent.GetType()} node {NodeIndex}";
}
