namespace ScottPlot.Plottables;

public abstract class AxisSpan : IPlottable, IHasLine, IHasFill, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();


    [Obsolete("set LegendText")]
    public Label Label { get => ObsoleteLabel; set => LegendText = value.Text; }
    private readonly Label ObsoleteLabel = new();

    public string LegendText { get; set; } = string.Empty;

    public LineStyle LineStyle { get; set; } = new();

    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, FillStyle);

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
