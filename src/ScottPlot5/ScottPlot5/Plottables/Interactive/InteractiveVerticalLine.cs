namespace ScottPlot.Plottables.Interactive;

public class InteractiveVerticalLine : LabelStyleProperties, IPlottable, IRenderLast, IHasInteractiveHandles, IHasLine, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public Cursor Cursor { get; set; } = Cursor.SizeWestEast;
    public LineStyle LineStyle { get; set; } = new LineStyle(2, Colors.Black);
    public bool LabelOppositeAxis { get; set; } = false;
    public double X { get; set; }
    public override LabelStyle LabelStyle { get; set; } = new();
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }
    public string LegendText { get; set; } = string.Empty;
    public string Text { get => LabelStyle.Text; set => LabelStyle.Text = value; }

    public AxisLimits GetAxisLimits() => AxisLimits.HorizontalOnly(X, X);

    public InteractiveHandle? GetHandle(CoordinateRect rect) =>
        IsVisible && rect.ContainsX(X) ? new InteractiveHandle(this, Cursor) : null;
    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point) => X = point.X;
    public virtual void PressHandle(InteractiveHandle handle) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        float x = Axes.GetPixelX(X);
        PixelLine line = new(x, rp.DataRect.Bottom, x, rp.DataRect.Top);
        LineStyle.Render(rp.Canvas, line, rp.Paint);
    }

    public void RenderLast(RenderPack rp)
    {
        if (LabelStyle.IsVisible == false || string.IsNullOrEmpty(LabelStyle.Text))
            return;

        // determine location
        float x = Axes.GetPixelX(X);

        // do not render if the axis line is outside the data area
        if (!rp.DataRect.ContainsX(x))
            return;

        float y = LabelOppositeAxis
            ? rp.DataRect.Top - LabelStyle.PixelPadding.Top
            : rp.DataRect.Bottom + LabelStyle.PixelPadding.Bottom;

        Alignment defaultAlignment = LabelOppositeAxis
            ? Alignment.LowerCenter
            : Alignment.UpperCenter;

        LabelStyle.Alignment = defaultAlignment;

        // draw label outside the data area
        rp.CanvasState.DisableClipping();

        LabelStyle.Render(rp.Canvas, new Pixel(x, y), rp.Paint);
    }

    public InteractiveVerticalLine()
    {
        LabelStyle.ForeColor = Colors.White;
        LabelStyle.Bold = true;
        LabelStyle.Padding = 5;
    }
}
