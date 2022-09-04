using SkiaSharp;

namespace ScottPlot.Axis.StandardAxes;

public class TopAxis : XAxisBase, IXAxis
{
    public Edge Edge => Edge.Top;

    public TopAxis()
    {
        TickGenerator = new TickGenerators.ScottPlot4.NumericTickGenerator(false);
    }

    public void Render(SKSurface surface, PixelRect dataRect)
    {
        var ticks = TickGenerator.GetVisibleTicks(Range);

        StandardRendering.DrawLabel(surface, dataRect, Edge, Label, Offset, PixelSize);
        StandardRendering.DrawTicksTop(surface, dataRect, Label.Color, Offset, ticks, this);
        StandardRendering.DrawFrame(surface, dataRect, Edge, Label.Color, Offset);
    }
}
