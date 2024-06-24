namespace ScottPlot.Plottables;

public class OhlcPlot(IOHLCSource data) : IPlottable
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    private readonly IOHLCSource Data = data;

    /// <summary>
    /// X position of each symbol is sourced from the OHLC's DateTime by default.
    /// If this option is enabled, X position will be an ascending integers starting at 0 with no gaps.
    /// </summary>
    public bool Sequential { get; set; } = false;

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

    public AxisLimits GetAxisLimits()
    {
        if (Sequential)
        {
            CoordinateRange yLimits = Data.GetLimitsY();
            CoordinateRange xLimits = new(0, Data.GetOHLCs().Count - 1);
            return new AxisLimits(xLimits, yLimits);
        }
        else
        {
            var limits = Data.GetLimits();
            List<OHLC> ohlcs = Data.GetOHLCs();
            if (ohlcs.Count == 0)
                return limits;

            double left = ohlcs.First().DateTime.ToOADate() - ohlcs.First().TimeSpan.TotalDays / 2;
            double right = ohlcs.Last().DateTime.ToOADate() + ohlcs.Last().TimeSpan.TotalDays / 2;

            return new(left, right, limits.Bottom, limits.Top);
        }
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        using SKPath risingPath = new();
        using SKPath fallingPath = new();

        IList<OHLC> ohlcs = Data.GetOHLCs();
        for (int i = 0; i < ohlcs.Count; i++)
        {
            OHLC ohlc = ohlcs[i];
            bool isRising = ohlc.Close >= ohlc.Open;
            SKPath path = isRising ? risingPath : fallingPath;

            float top = Axes.GetPixelY(ohlc.High);
            float bottom = Axes.GetPixelY(ohlc.Low);

            float center, left, right;

            if (Sequential == false)
            {
                double centerNumber = NumericConversion.ToNumber(ohlc.DateTime);
                center = Axes.GetPixelX(centerNumber);
                double halfWidthNumber = ohlc.TimeSpan.TotalDays / 2 * SymbolWidth;
                left = Axes.GetPixelX(centerNumber - halfWidthNumber);
                right = Axes.GetPixelX(centerNumber + halfWidthNumber);
            }
            else
            {
                center = Axes.GetPixelX(i);
                left = Axes.GetPixelX(i - (float)SymbolWidth / 2);
                right = Axes.GetPixelX(i + (float)SymbolWidth / 2);
            }

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
