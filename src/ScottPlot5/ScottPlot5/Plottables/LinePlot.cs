namespace ScottPlot.Plottables;

public class LinePlot : IPlottable
{
    public Line Line { get; set; } = Line.Default;
    public string Label { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, Line.Style);
    public AxisLimits GetAxisLimits()
    {
        CoordinateRect boundingRect = new(Line.Start, Line.End);
        return new AxisLimits(boundingRect);
    }

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        Line.Style.ApplyToPaint(paint);

        using SKPath path = new();
        path.MoveTo(Axes.GetPixel(Line.Start).ToSKPoint());
        path.LineTo(Axes.GetPixel(Line.End).ToSKPoint());

        rp.Canvas.DrawPath(path, paint);
    }
}
