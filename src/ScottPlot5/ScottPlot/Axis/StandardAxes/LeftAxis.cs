using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class LeftAxis : YAxisBase, IYAxis
{
    public Edge Edge { get; } = Edge.Left;

    public LeftAxis()
    {
        TickGenerator = new TickGenerators.ScottPlot4.NumericTickGenerator(true);
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        var ticks = TickGenerator.GetVisibleTicks(Range);
        StandardRendering.DrawLabel(surface, dataRect, Edge, Label, Offset, PixelSize);
        StandardRendering.DrawTicksLeft(surface, dataRect, Label.Color, Offset, ticks, this);
        StandardRendering.DrawFrame(surface, dataRect, Edge, Label.Color, Offset);
    }
}
