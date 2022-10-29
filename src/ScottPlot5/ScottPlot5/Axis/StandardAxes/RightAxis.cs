using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class RightAxis : YAxisBase, IYAxis
{
    public override Edge Edge { get; } = Edge.Right;

    public RightAxis()
    {
        TickGenerator = new TickGenerators.ScottPlot4.NumericTickGenerator(true);
    }
}
