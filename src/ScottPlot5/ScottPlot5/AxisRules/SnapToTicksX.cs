namespace ScottPlot.AxisRules;

public class SnapToTicksX(IXAxis xAxis) : IAxisRule
{
    public readonly IXAxis XAxis = xAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (beforeLayout)
            return;

        var ticks = XAxis.TickGenerator.Ticks.Where(tick => tick.IsMajor).Select(x => x.Position);
        if (ticks.Count() < 2)
            return;

        double tickDelta = ticks.Skip(1).First() - ticks.First();
        XAxis.Range.Set(ticks.Min() - tickDelta, ticks.Max() + tickDelta);
    }
}
