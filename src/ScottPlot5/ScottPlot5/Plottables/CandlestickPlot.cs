namespace ScottPlot.Plottables;

public class CandlestickPlot(IOHLCSource data) : IPlottable
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public IOHLCSource Data { get; } = data;

    /// <summary>
    /// X position of candles is sourced from the OHLC's DateTime by default.
    /// If this option is enabled, X position will be an ascending integers starting at 0 with no gaps.
    /// </summary>
    public bool Sequential { get; set; } = false;

    /// <summary>
    /// Fractional width of the candle symbol relative to its time span
    /// </summary>
    public double SymbolWidth = .8;

    public LineStyle RisingLineStyle { get; } = new()
    {
        Color = Color.FromHex("#089981"),
        Width = 2,
    };

    public LineStyle FallingLineStyle { get; } = new()
    {
        Color = Color.FromHex("#f23645"),
        Width = 2,
    };

    public FillStyle RisingFillStyle { get; } = new()
    {
        Color = Color.FromHex("#089981"),
    };

    public FillStyle FallingFillStyle { get; } = new()
    {
        Color = Color.FromHex("#f23645"),
    };

    public Color RisingColor
    {
        set
        {
            RisingLineStyle.Color = value;
            RisingFillStyle.Color = value;
        }
    }

    public Color FallingColor
    {
        set
        {
            FallingLineStyle.Color = value;
            FallingFillStyle.Color = value;
        }
    }

    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    public AxisLimits GetAxisLimits()
    {
        AxisLimits limits = Data.GetLimits(); // TODO: Data.GetSequentialLimits()

        if (Sequential)
        {
            return new AxisLimits(
                left: -0.5, // extra to account for body size
                right: Data.GetOHLCs().Count - 1 + 0.5, // extra to account for body size
                bottom: limits.Bottom,
                top: limits.Top);
        }

        var ohlcs = Data.GetOHLCs();
        if (ohlcs.Count == 0)
            return limits;

        double left = ohlcs.First().DateTime.ToOADate() - ohlcs.First().TimeSpan.TotalDays / 2;
        double right = ohlcs.Last().DateTime.ToOADate() + ohlcs.Last().TimeSpan.TotalDays / 2;

        return new(left, right, limits.Bottom, limits.Top);
    }

    public CoordinateRange GetPriceRangeInView()
    {
        var ohlcs = Data.GetOHLCs();
        if (ohlcs.Count == 0)
            return CoordinateRange.NoLimits;

        int minIndexInView = (int)NumericConversion.Clamp(Axes.XAxis.Min, 0, ohlcs.Count - 1);
        int maxIndexInView = (int)NumericConversion.Clamp(Axes.XAxis.Max, 0, ohlcs.Count - 1);
        return Data.GetPriceRange(minIndexInView, maxIndexInView);
    }

    public (int index, OHLC ohlc)? GetOhlcNearX(double x)
    {
        int ohlcIndex = (int)Math.Round(x);
        return (ohlcIndex >= 0 && ohlcIndex < Data.Count)
            ? (ohlcIndex, Data.GetOHLCs()[ohlcIndex])
            : null;
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        var ohlcs = Data.GetOHLCs();
        for (int i = 0; i < ohlcs.Count; i++)
        {
            OHLC ohlc = ohlcs[i];
            bool isRising = ohlc.Close >= ohlc.Open;
            LineStyle lineStyle = isRising ? RisingLineStyle : FallingLineStyle;
            FillStyle fillStyle = isRising ? RisingFillStyle : FallingFillStyle;

            float top = Axes.GetPixelY(ohlc.High);
            float bottom = Axes.GetPixelY(ohlc.Low);

            float center, xPxLeft, xPxRight;
            if (Sequential == false)
            {
                double centerNumber = NumericConversion.ToNumber(ohlc.DateTime);
                center = Axes.GetPixelX(centerNumber);
                double halfWidthNumber = ohlc.TimeSpan.TotalDays / 2 * SymbolWidth;
                xPxLeft = Axes.GetPixelX(centerNumber - halfWidthNumber);
                xPxRight = Axes.GetPixelX(centerNumber + halfWidthNumber);
            }
            else
            {
                center = Axes.GetPixelX(i);
                xPxLeft = Axes.GetPixelX(i - (float)SymbolWidth / 2);
                xPxRight = Axes.GetPixelX(i + (float)SymbolWidth / 2);
            }

            // do not render OHLCs off the screen
            if (xPxRight < rp.DataRect.Left || xPxLeft > rp.DataRect.Right)
                continue;

            float yPxOpen = Axes.GetPixelY(ohlc.Open);
            float yPxClose = Axes.GetPixelY(ohlc.Close);

            // low/high line
            PixelLine verticalLine = new(center, top, center, bottom);
            Drawing.DrawLine(rp.Canvas, paint, verticalLine, lineStyle);

            // open/close body
            bool barIsAtLeastOnePixelWide = xPxRight - xPxLeft > 1;
            if (barIsAtLeastOnePixelWide)
            {
                PixelRangeX xPxRange = new(xPxLeft, xPxRight);
                PixelRangeY yPxRange = new(Math.Min(yPxOpen, yPxClose), Math.Max(yPxOpen, yPxClose));
                PixelRect rect = new(xPxRange, yPxRange);
                if (yPxOpen != yPxClose)
                {
                    fillStyle.Render(rp.Canvas, rect, paint);
                }
                else
                {
                    lineStyle.Render(rp.Canvas, rect.BottomLine, paint);
                }
            }
        }
    }
}
