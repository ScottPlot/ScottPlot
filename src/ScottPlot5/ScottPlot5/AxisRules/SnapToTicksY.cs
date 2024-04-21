namespace ScottPlot.AxisRules;

public class SnapToTicksY(IYAxis yAxis) : IAxisRule
{
    public readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (beforeLayout)
            return;

        // do not attempt to set limits according to ticks while the window is resizing
        if (rp.Plot.RenderManager.LastRender.DataRect != rp.DataRect)
            return;

        YAxis.RegenerateTicks(new PixelLength(rp.FigureRect.Height));
        var ticks = YAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        if (ticks.Count() < 2)
            return;

        double tickDelta = ticks.Skip(1).First() - ticks.First();
        YAxis.Range.Set(ticks.Min() - tickDelta, ticks.Max() + tickDelta);
    }
}
