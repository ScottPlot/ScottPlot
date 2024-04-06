namespace ScottPlot.Plottables;

public class FunctionPlot : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    /// <summary>
    /// Default horizontal range to use when autoscaling
    /// </summary>
    public CoordinateRange XRange { get; set; } = new(-10, 10);

    public string? Label { get; set; }
    public LineStyle LineStyle { get; } = new();
    IFunctionSource Source { get; set; }
    AxisLimits LastRenderLimits { get; set; } = AxisLimits.NoLimits;

    public FunctionPlot(IFunctionSource source)
    {
        Source = source;
    }
    private double MinX => Math.Max(Source.RangeX.Min.FiniteCoallesce(Axes.XAxis.Min), Axes.XAxis.Min);
    private double MaxX => Math.Min(Source.RangeX.Max.FiniteCoallesce(Axes.XAxis.Max), Axes.XAxis.Max);


    public IEnumerable<LegendItem> LegendItems => EnumerableExtensions.One(
        new LegendItem
        {
            Label = Label,
            Marker = MarkerStyle.None,
            Line = LineStyle,
        });

    public AxisLimits GetAxisLimits()
    {
        return LastRenderLimits;
    }

    public void Render(RenderPack rp)
    {
        var unitsPerPixel = Axes.XAxis.GetCoordinateDistance(1, rp.DataRect);

        double max = double.MinValue;
        double min = double.MaxValue;

        using SKPath path = new();
        bool penIsDown = false;
        for (double x = MinX; x <= MaxX; x += unitsPerPixel)
        {
            double y = Source.Get(x);
            if (y.IsInfiniteOrNaN())
            {
                penIsDown = false; // Picking up pen allows us to skip over regions where the function is undefined
                continue;
            }

            max = Math.Max(max, y);
            min = Math.Min(min, y);

            Pixel px = Axes.GetPixel(new(x, y));

            if (penIsDown)
            {
                path.LineTo(px.ToSKPoint());
            }
            else
            {
                path.MoveTo(px.ToSKPoint());
                penIsDown = true;
            }
        }

        using SKPaint paint = new();
        LineStyle.ApplyToPaint(paint);

        rp.Canvas.DrawPath(path, paint);

        LastRenderLimits = new AxisLimits(XRange.Min, XRange.Max, min, max);
    }
}
