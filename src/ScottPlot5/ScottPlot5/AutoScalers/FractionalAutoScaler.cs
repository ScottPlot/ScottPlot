namespace ScottPlot.AutoScalers;

public class FractionalAutoScaler : IAutoScaler
{
    public readonly double LeftFraction;
    public readonly double RightFraction;
    public readonly double BottomFraction;
    public readonly double TopFraction;
    public bool InvertedX { get; set; } = false;
    public bool InvertedY { get; set; } = false;

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
            if (!plottable.IsVisible)
                continue;

            AxisLimits limits = plottable.GetAxisLimits();
            plottable.Axes.XAxis.Range.Expand(limits.XRange);
            plottable.Axes.YAxis.Range.Expand(limits.YRange);
        }

        foreach (IAxis xAxis in xAxes)
        {
            double left = xAxis.Range.Min - (xAxis.Range.Span * LeftFraction);
            double right = xAxis.Range.Max + (xAxis.Range.Span * RightFraction);
            if (NumericConversion.IsReal(left) && NumericConversion.IsReal(right))
            {
                if (InvertedX)
                {
                    xAxis.Range.Set(right, left);
                }
                else
                {
                    xAxis.Range.Set(left, right);
                }
            }
        }

        foreach (IYAxis yAxis in yAxes)
        {
            double bottom = yAxis.Range.Min - (yAxis.Range.Span * BottomFraction);
            double top = yAxis.Range.Max + (yAxis.Range.Span * TopFraction);
            if (NumericConversion.IsReal(bottom) && NumericConversion.IsReal(top))
            {
                if (InvertedY)
                {
                    yAxis.Range.Set(top, bottom);
                }
                else
                {
                    yAxis.Range.Set(bottom, top);
                }
            }
        }
    }

    public AxisLimits GetAxisLimits(Plot plot, IXAxis xAxis, IYAxis yAxis)
    {
        AxisLimits dataLimits = plot.Axes.GetDataLimits(xAxis, yAxis);
        ExpandingAxisLimits limits = new(dataLimits);

        if (!limits.IsRealX)
        {
            limits.SetX(-10, 10);
        }

        if (!limits.IsRealY)
        {
            limits.SetY(-10, 10);
        }

        if (limits.Left == limits.Right)
        {
            limits.SetX(limits.Left - 1, limits.Right + 1);

            if (limits.Left == limits.Right)
            {
                limits.Left = NumericConversion.DecrementLargeDouble(limits.Left);
                limits.Right = NumericConversion.IncrementLargeDouble(limits.Right);
            }
        }

        if (limits.Bottom == limits.Top)
        {
            limits.SetY(limits.Bottom - 1, limits.Top + 1);

            if (limits.Bottom == limits.Top)
            {
                limits.Bottom = NumericConversion.DecrementLargeDouble(limits.Bottom);
                limits.Top = NumericConversion.IncrementLargeDouble(limits.Top);
            }
        }

        AxisLimits newLimits = new(
            left: limits.Left - (limits.HorizontalSpan * LeftFraction),
            right: limits.Right + (limits.HorizontalSpan * RightFraction),
            bottom: limits.Bottom - (limits.VerticalSpan * BottomFraction),
            top: limits.Top + (limits.VerticalSpan * TopFraction));

        if (!newLimits.IsReal || !newLimits.HasArea)
            throw new InvalidOperationException("limits returned by the autoscaler must always be in a good state");

        if (InvertedX)
            newLimits = newLimits.InvertedHorizontally();

        if (InvertedY)
            newLimits = newLimits.InvertedVertically();

        return newLimits;
    }
}
