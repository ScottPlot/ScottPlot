using ScottPlot.Axis;

namespace ScottPlot.Plottables;

public class OHLCPlot : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = Axis.Axes.Default;
    public readonly DataSources.IOHLCSource Data;

    public LineStyle GrowingStyle { get; } = new() { Color = Color.FromHex("#089981"), Width = 2 };
    public LineStyle FallingStyle { get; } = new() { Color = Color.FromHex("#f23645"), Width = 2 };

    /// <summary>
    /// Width (in pixels) of each symbol on the chart
    /// </summary>
    public int Width { get; set; } = 10; // TODO: OHLCs should store their own time span used to calculate symbol width

    public OHLCPlot(DataSources.IOHLCSource data)
    {
        Data = data;
    }

    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    public AxisLimits GetAxisLimits() => Data.GetLimits();

    public void Render(SKSurface surface)
    {
        using SKPaint paint = new();
        using SKPath growingPath = new();
        using SKPath fallingPath = new();

        foreach (OHLC ohlc in Data.GetOHLCs())
        {
            SKPath path = ohlc.Close >= ohlc.Open ? growingPath : fallingPath;

            float center = Axes.GetPixelX(ohlc.DateTime.ToNumber());
            float top = Axes.GetPixelY(ohlc.High);
            float bottom = Axes.GetPixelY(ohlc.Low);

            path.MoveTo(center, top);
            path.LineTo(center, bottom);

            float open = Axes.GetPixelY(ohlc.Open);
            float close = Axes.GetPixelY(ohlc.Close);

            path.MoveTo(center - Width / 2, open);
            path.LineTo(center, open);

            path.MoveTo(center, close);
            path.LineTo(center + Width / 2, close);
        }

        GrowingStyle.ApplyToPaint(paint);
        surface.Canvas.DrawPath(growingPath, paint);

        FallingStyle.ApplyToPaint(paint);
        surface.Canvas.DrawPath(fallingPath, paint);
    }
}
