using ScottPlot.Axis;

namespace ScottPlot.Plottables;

public class CandlestickPlot : IPlottable
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = Axis.Axes.Default;

    private readonly DataSources.IOHLCSource Data;

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

    public CandlestickPlot(DataSources.IOHLCSource data)
    {
        Data = data;
    }

    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        foreach (IOHLC ohlc in Data.GetOHLCs())
        {
            bool isRising = ohlc.Close >= ohlc.Open;
            LineStyle lineStyle = isRising ? RisingLineStyle : FallingLineStyle;
            FillStyle fillStlye = isRising ? RisingFillStyle : FallingFillStyle;

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

            // center line
            using SKPath path = new();
            path.MoveTo(center, top);
            path.LineTo(center, bottom);

            lineStyle.ApplyToPaint(paint);
            rp.Canvas.DrawPath(path, paint);

            // rectangle
            SKRect rect = new(left, Math.Max(open, close), right, Math.Min(open, close));
            fillStlye.ApplyToPaint(paint);
            rp.Canvas.DrawRect(rect, paint);
        }
    }
}
