using ScottPlot.Renderable;

namespace ScottPlot;

public interface IAxis
{
    int AxisIndex { get; set; }
    Edge Edge { get; set; }
}
