namespace ScottPlot.AutoScalers;

internal class FractionalAutoScaler : IAutoScaler
{
    public readonly double LeftFraction;
    public readonly double RightFraction;
    public readonly double BottomFraction;
    public readonly double TopFraction;

    /// <summary>
    /// Tightly fit the data with no padding
    /// </summary>
    public FractionalAutoScaler()
    {
        LeftFraction = 0;
        RightFraction = 0;
        BottomFraction = 0;
        TopFraction = 0;
    }

    /// <summary>
    /// Pad the data area with the given fractions of whitespace.
    /// A value of 0.1 means 10% padding (5% on each side of the data area).
    /// </summary>
    public FractionalAutoScaler(double horizontal, double vertical)
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

    public AxisLimits GetAxisLimits(IEnumerable<IPlottable> plottables, IXAxis xAxis, IYAxis yAxis)
    {
        ExpandingAxisLimits limits = new();
        foreach (IPlottable plottable in plottables.Where(x => x.Axes.XAxis == xAxis && x.Axes.YAxis == yAxis))
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
