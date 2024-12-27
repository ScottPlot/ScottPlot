namespace ScottPlot.AxisRules;

public class MaximumBoundary(IXAxis xAxis, IYAxis yAxis, AxisLimits limits) : IAxisRule
{
    readonly IXAxis XAxis = xAxis;
    readonly IYAxis YAxis = yAxis;
    public AxisLimits Limits { get; set; } = limits;

    public void Apply(RenderPack rp, bool beforeLayout)
    {
        if (XAxis.IsInverted())
        {
            if (XAxis.Range.Min > Limits.XRange.Max)
            {
                XAxis.Range.Min = Limits.XRange.Max;
            }

            if (XAxis.Range.Max < Limits.XRange.Min)
            {
                XAxis.Range.Max = Limits.XRange.Min;
            }
        }
        else
        {
            if (XAxis.Range.Max > Limits.XRange.Max)
            {
                XAxis.Range.Max = Limits.XRange.Max;
            }

            if (XAxis.Range.Min < Limits.XRange.Min)
            {
                XAxis.Range.Min = Limits.XRange.Min;
            }
        }

        if (YAxis.IsInverted())
        {
            if (YAxis.Range.Min > Limits.YRange.Max)
            {
                YAxis.Range.Min = Limits.YRange.Max;
            }

            if (YAxis.Range.Max < Limits.YRange.Min)
            {
                YAxis.Range.Max = Limits.YRange.Min;
            }
        }
        else
        {
            if (YAxis.Range.Max > Limits.YRange.Max)
            {
                YAxis.Range.Max = Limits.YRange.Max;
            }

            if (YAxis.Range.Min < Limits.YRange.Min)
            {
                YAxis.Range.Min = Limits.YRange.Min;
            }
        }
    }
}
