namespace ScottPlot.Plottables;

public abstract class AxisSpan : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();


    public readonly Label Label = new();
    public LineStyle LineStyle { get; set; } = new();
    public FillStyle FillStyle { get; set; } = new();

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label.Text, FillStyle);

    public abstract AxisLimits GetAxisLimits();

    public abstract void Render(RenderPack rp);

    public bool IsDraggable { get; set; } = false;
    public bool IsResizable { get; set; } = false;
    public abstract AxisSpanUnderMouse? UnderMouse(CoordinateRect rect);
    public abstract void DragTo(AxisSpanUnderMouse start, Coordinates mouseNow);

    protected void Render(RenderPack rp, PixelRect rect)
    {
        using SKPaint paint = new();
        Drawing.FillRectangle(rp.Canvas, rect, paint, FillStyle);
        Drawing.DrawRectangle(rp.Canvas, rect, paint, LineStyle);
    }
}
