namespace ScottPlot.AxisRules;

public class SquarePreserveX : IAxisRule
{
    readonly IXAxis XAxis;
    readonly IYAxis YAxis;

    public SquarePreserveX(IXAxis xAxis, IYAxis yAxis)
    {
        XAxis = xAxis;
        YAxis = yAxis;
    }

    public void Apply(RenderPack rp)
    {
        double unitsPerPxX = XAxis.Width / rp.DataRect.Width;
        double halfHeight = rp.DataRect.Height / 2 * unitsPerPxX;
        double yMin = YAxis.Range.Center - halfHeight;
        double yMax = YAxis.Range.Center + halfHeight;
        YAxis.Range.Set(yMin, yMax);
    }
}
