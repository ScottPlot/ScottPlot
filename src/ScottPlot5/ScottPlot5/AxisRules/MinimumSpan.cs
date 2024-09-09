namespace ScottPlot.AxisRules;

public class MinimumSpan(IXAxis xAxis, IYAxis yAxis, double xSpan, double ySpan) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;

    public double XSpan = xSpan;
    public double YSpan = ySpan;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (XAxis.Span < XSpan)
        {
            double xMin = XAxis.Center - XSpan / 2;
            double xMax = XAxis.Center + XSpan / 2;
            XAxis.SetRange(xMin, xMax);
        }

        if (YAxis.Span < YSpan)
        {
            double yMin = YAxis.Center - YSpan / 2;
            double yMax = YAxis.Center + YSpan / 2;
            YAxis.SetRange(yMin, yMax);
        }
    }
}
