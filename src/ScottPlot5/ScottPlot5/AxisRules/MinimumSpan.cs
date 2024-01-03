namespace ScottPlot.AxisRules;

public class MinimumSpan : IAxisRule
{
    readonly IXAxis XAxis;
    readonly IYAxis YAxis;

    public double XSpan;
    public double YSpan;

    public MinimumSpan(IXAxis xAxis, IYAxis yAxis, double xSpan = double.Epsilon, double ySpan = double.Epsilon)
    {
        XAxis = xAxis;
        YAxis = yAxis;
        XSpan = xSpan;
        YSpan = ySpan;
    }

    public void Apply(RenderPack rp)
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
