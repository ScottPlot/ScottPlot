namespace ScottPlot.Plottables.Interactive;

public class InteractiveVerticalLine : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public Cursor Cursor { get; set; } = Cursor.SizeWestEast;
    public LineStyle LineStyle { get; } = new LineStyle(2, Colors.Black);

    public double X { get; set; }
    public AxisLimits GetAxisLimits() => AxisLimits.HorizontalOnly(X, X);

    public InteractiveHandle? GetHandle(CoordinateRect rect) =>
        rect.ContainsX(X) ? new InteractiveHandle(this, Cursor) : null;
    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point) => X = point.X;
    public virtual void PressHandle(InteractiveHandle handle, Coordinates point) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        float x = Axes.GetPixelX(X);
        PixelLine line = new(x, rp.DataRect.Bottom, x, rp.DataRect.Top);
        LineStyle.Render(rp.Canvas, line, rp.Paint);
    }
}
