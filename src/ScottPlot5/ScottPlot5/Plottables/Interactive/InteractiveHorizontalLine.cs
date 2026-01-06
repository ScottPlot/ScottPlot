namespace ScottPlot.Plottables.Interactive;

public class InteractiveHorizontalLine : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public Cursor Cursor { get; set; } = Cursor.SizeNorthSouth;
    public LineStyle LineStyle { get; } = new LineStyle(2, Colors.Black);

    public double Y { get; set; }
    public AxisLimits GetAxisLimits() => AxisLimits.VerticalOnly(Y, Y);

    public InteractiveHandle? GetHandle(CoordinateRect rect) =>
        rect.ContainsY(Y) ? new InteractiveHandle(this, Cursor) : null;
    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point) => Y = point.Y;
    public virtual void PressHandle(InteractiveHandle handle) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        float y = Axes.GetPixelY(Y);
        PixelLine line = new(rp.DataRect.Left, y, rp.DataRect.Right, y);
        LineStyle.Render(rp.Canvas, line, rp.Paint);
    }
}
