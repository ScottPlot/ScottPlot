namespace ScottPlot.Plottables;

public class OhlcPlot(IOHLCSource data) : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public object Tag { get; set; } = new();
    public IAxes Axes { get; set; } = new Axes();

    private readonly IOHLCSource Data = data;

    /// <summary>
    /// Fractional width of the OHLC symbol relative to its time span
    /// </summary>
    public double SymbolWidth = .8;

    public LineStyle RisingStyle { get; } = new()
    {
        Color = Color.FromHex("#089981"),
        Width = 2,
    };

    public LineStyle FallingStyle { get; } = new()
    {
        Color = Color.FromHex("#f23645"),
        Width = 2,
    };

    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        using SKPath risingPath = new();
        using SKPath fallingPath = new();

        foreach (OHLC ohlc in Data.GetOHLCs())
        {
            bool isRising = ohlc.Close >= ohlc.Open;
            SKPath path = isRising ? risingPath : fallingPath;

            float center = Axes.GetPixelX(ohlc.DateTime.ToNumber());
            float top = Axes.GetPixelY(ohlc.High);
            float bottom = Axes.GetPixelY(ohlc.Low);

            TimeSpan halfWidth = new((long)(ohlc.TimeSpan.Ticks * SymbolWidth / 2));
            DateTime leftTime = ohlc.DateTime - halfWidth;
            DateTime rightTime = ohlc.DateTime + halfWidth;
            float left = Axes.GetPixelX(leftTime.ToNumber());
            float right = Axes.GetPixelX(rightTime.ToNumber());

            float open = Axes.GetPixelY(ohlc.Open);
            float close = Axes.GetPixelY(ohlc.Close);

            // do not render OHLCs off the screen
            if (right < rp.DataRect.Left || left > rp.DataRect.Right)
                continue;

            // center line
            path.MoveTo(center, top);
            path.LineTo(center, bottom);

            // left peg
            path.MoveTo(left, open);
            path.LineTo(center, open);

            // right peg
            path.MoveTo(center, close);
            path.LineTo(right, close);
        }

        RisingStyle.ApplyToPaint(paint);
        rp.Canvas.DrawPath(risingPath, paint);

        FallingStyle.ApplyToPaint(paint);
        rp.Canvas.DrawPath(fallingPath, paint);
    }
}
