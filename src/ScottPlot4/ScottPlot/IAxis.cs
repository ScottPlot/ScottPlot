using ScottPlot.Renderable;

namespace ScottPlot;

public interface IAxis
{
    int AxisIndex { get; }
    Edge Edge { get; }
}
