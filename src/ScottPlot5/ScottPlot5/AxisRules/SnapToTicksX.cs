namespace ScottPlot.AxisRules;

public class SnapToTicksX(IXAxis xAxis) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (beforeLayout)
            return;

        // do not attempt to set limits according to ticks while the window is resizing
        if (rp.Plot.RenderManager.RenderCount > 0 && rp.Plot.RenderManager.LastRender.DataRect != rp.DataRect)
            return;

        XAxis.RegenerateTicks(new PixelLength(rp.FigureRect.Width));
        var ticks = XAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        if (ticks.Count() < 2)
            return;

        double tickDelta = ticks.Skip(1).First() - ticks.First();
        XAxis.Range.Set(ticks.Min() - tickDelta, ticks.Max() + tickDelta);
    }
}
