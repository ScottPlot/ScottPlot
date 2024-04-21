namespace ScottPlot.AxisRules;

public class MinimumSpan(IXAxis xAxis, IYAxis yAxis, double xSpan, double ySpan) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;

    public double XSpan = xSpan;
    public double YSpan = ySpan;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (XAxis.Range.Span < XSpan)
        {
            double xMin = XAxis.Range.Center - XSpan / 2;
            double xMax = XAxis.Range.Center + XSpan / 2;
            XAxis.Range.Set(xMin, xMax);
        }

        if (YAxis.Range.Span < YSpan)
        {
            double yMin = YAxis.Range.Center - YSpan / 2;
            double yMax = YAxis.Range.Center + YSpan / 2;
            YAxis.Range.Set(yMin, yMax);
        }
    }
}
