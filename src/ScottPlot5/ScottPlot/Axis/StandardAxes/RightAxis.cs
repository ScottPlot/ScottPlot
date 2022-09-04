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
        StandardRendering.DrawLabel(surface, dataRect, Edge, Label, Offset, PixelSize);
        StandardRendering.DrawTicksRight(surface, dataRect, Label.Color, Offset, ticks, this);
        StandardRendering.DrawFrame(surface, dataRect, Edge, Label.Color, Offset);
    }
}
