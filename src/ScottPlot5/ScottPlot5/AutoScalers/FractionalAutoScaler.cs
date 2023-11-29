namespace ScottPlot.AutoScalers;

internal class FractionalAutoScaler : IAutoScaler
{
    public readonly double LeftFraction;
    public readonly double RightFraction;
    public readonly double BottomFraction;
    public readonly double TopFraction;

    /// <summary>
    /// Pad the data area with the given fractions of whitespace.
    /// A value of 0.1 means 10% padding (5% on each side of the data area).
    /// </summary>
    public FractionalAutoScaler(double horizontal = .1, double vertical = .15)
    {
        LeftFraction = horizontal / 2;
        RightFraction = horizontal / 2;
        BottomFraction = vertical / 2;
        TopFraction = vertical / 2;
    }

    /// <summary>
    /// Pad each side of the data area with the exact fraction of whitespace.
    /// 0.05 means 5% whitespace on that side of the data area.
    /// </summary>
    public FractionalAutoScaler(double left, double right, double bottom, double top)
    {
        LeftFraction = left;
        RightFraction = right;
        BottomFraction = bottom;
        TopFraction = top;
    }

    public void AutoScaleAll(IEnumerable<IPlottable> plottables)
    {
        IEnumerable<IXAxis> xAxes = plottables.Select(x => x.Axes.XAxis).Distinct();
        IEnumerable<IYAxis> yAxes = plottables.Select(x => x.Axes.YAxis).Distinct();

        xAxes.ToList().ForEach(x => x.Range.Reset());
        yAxes.ToList().ForEach(x => x.Range.Reset());

        foreach (IPlottable plottable in plottables)
        {
            AxisLimits limits = plottable.GetAxisLimits();
            plottable.Axes.XAxis.Range.Expand(limits.XRange);
            plottable.Axes.YAxis.Range.Expand(limits.YRange);
        }

        foreach (IAxis xAxis in xAxes)
        {
            double left = xAxis.Range.Min - (xAxis.Range.Span * LeftFraction);
            double right = xAxis.Range.Max + (xAxis.Range.Span * RightFraction);
            xAxis.Range.Set(left, right);
        }

        foreach (IYAxis yAxis in yAxes)
        {
            double bottom = yAxis.Range.Min - (yAxis.Range.Span * BottomFraction);
            double top = yAxis.Range.Max + (yAxis.Range.Span * TopFraction);
            yAxis.Range.Set(bottom, top);
        }
    }

    public AxisLimits GetAxisLimits(Plot plot, IXAxis xAxis, IYAxis yAxis)
    {
        ExpandingAxisLimits limits = new();

        foreach (IPlottable plottable in plot.PlottableList.Where(x => x.Axes.XAxis == xAxis && x.Axes.YAxis == yAxis))
        {
            limits.Expand(plottable.GetAxisLimits());
        }

        return new AxisLimits(
            left: limits.Left - (limits.HorizontalSpan * LeftFraction),
            right: limits.Right + (limits.HorizontalSpan * RightFraction),
            bottom: limits.Bottom - (limits.VerticalSpan * BottomFraction),
            top: limits.Top + (limits.VerticalSpan * TopFraction));
    }
}
