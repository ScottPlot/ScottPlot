using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class BottomAxis : XAxisBase, IXAxis
{
    public Edge Edge => Edge.Bottom;

    public BottomAxis()
    {
        TickGenerator = new TickGenerators.ScottPlot4.NumericTickGenerator(false);
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        var ticks = TickGenerator.GetVisibleTicks(Range);

        StandardRendering.DrawLabel(surface, dataRect, Edge, Label, Offset, PixelSize);
        StandardRendering.DrawTicksBottom(surface, dataRect, Label.Color, Offset, ticks, this);
        StandardRendering.DrawFrame(surface, dataRect, Edge, Label.Color, Offset);
    }
}
