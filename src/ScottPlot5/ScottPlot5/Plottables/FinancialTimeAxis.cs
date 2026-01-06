namespace ScottPlot.Plottables;

/// <summary>
/// This plottable renders date tick labels for financial charts where
/// data is displayed sequentially along the horizontal axis despite
/// DateTimes not being evenly spaced (e.g., data may include gaps)
/// </summary>
public class FinancialTimeAxis(DateTime[] dateTimes) : IPlottable
{
    private int startIndex = -1;
    private int candlesToSkip = 0;
    public DateTime[] DateTimes { get; set; } = dateTimes;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    double widthOfCandleInPixels;
    const string labelFormat = "   HH:mm:ss   ";

    public LabelStyle LabelStyle { get; set; } = new()
    {
        FontSize = 14,
        Alignment = Alignment.UpperCenter,
    };

    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    public virtual void Render(RenderPack rp)
    {
        if (DateTimes.Length == 0)
            return;

        // allow drawing outside the data area
        rp.CanvasState.DisableClipping();

        // get the best tick generator given the field of view
        int minIndexInView = (int)(Math.Max(0, Axes.XAxis.Range.Min));
        int maxIndexInView = (int)(Math.Min(DateTimes.Length - 1, Axes.XAxis.Range.Max));
        if (maxIndexInView <= minIndexInView) return;
        TimeSpan timeSpanInView = DateTimes[maxIndexInView] - DateTimes[minIndexInView];
        widthOfCandleInPixels = rp.DataRect.Width / Axes.XAxis.Range.Span;
        IFinancialTickGenerator tickGenerator = GetBestTickGenerator(timeSpanInView, rp.DataRect.Width, rp.Paint);
        List<(int, string)> ticks = tickGenerator.GetTicks(DateTimes, minIndexInView, maxIndexInView);
        startIndex = ticks.First().Item1;

        // render each tick label
        foreach ((int x, string label) in ticks)
        {
            Pixel px = new(Axes.XAxis.GetPixel(x, rp.DataRect), rp.DataRect.Bottom);
            LabelStyle.Render(rp.Canvas, px, rp.Paint, label);
        }
    }

    private IFinancialTickGenerator GetBestTickGenerator(TimeSpan timeSpan, float widthInPixels, Paint paint)
    {
        var maxWidth = LabelStyle.Measure(labelFormat, paint).Size.Width;
        var newCandlesToSkip = (int)Math.Ceiling(maxWidth / widthOfCandleInPixels);
        if (candlesToSkip != newCandlesToSkip)
        {
            startIndex = -1;
            candlesToSkip = newCandlesToSkip;
        }
        return new TickGenerators.Financial.EveryNthUnit(candlesToSkip, startIndex);
    }
}
