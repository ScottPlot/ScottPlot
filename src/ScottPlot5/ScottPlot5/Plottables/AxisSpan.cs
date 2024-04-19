namespace ScottPlot.Plottables;

public abstract class AxisSpan : IPlottable, IHasLine, IHasFill
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();


    public string Label { get; set; } = string.Empty;

    public LineStyle LineStyle { get; } = new();

    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, FillStyle);

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
