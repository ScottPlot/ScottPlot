using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class RightAxis : YAxisBase, IYAxis
{
    public Edge Edge { get; } = Edge.Right;

    public RightAxis()
    {
        TickGenerator = new TickGenerators.ScottPlot4.NumericTickGenerator(true);
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        var ticks = TickGenerator.GetVisibleTicks(Range);
        AxisRendering.DrawLabel(surface, dataRect, Edge, Label, Offset, PixelSize);
        AxisRendering.DrawTicksRight(surface, dataRect, Label.Color, Offset, ticks, this);
        AxisRendering.DrawFrame(surface, dataRect, Edge, Label.Color, Offset);
    }
}
