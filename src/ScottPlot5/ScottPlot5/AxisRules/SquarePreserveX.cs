namespace ScottPlot.AxisRules;

public class SquarePreserveX(IXAxis xAxis, IYAxis yAxis) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        // rules that refer to the DataRect must wait for the layout to occur
        if (beforeLayout)
            return;

        double unitsPerPxX = XAxis.Width / rp.DataRect.Width;
        double halfHeight = rp.DataRect.Height / 2 * unitsPerPxX;
        double yMin = YAxis.Range.Center - halfHeight;
        double yMax = YAxis.Range.Center + halfHeight;
        YAxis.Range.Set(yMin, yMax);
    }
}
