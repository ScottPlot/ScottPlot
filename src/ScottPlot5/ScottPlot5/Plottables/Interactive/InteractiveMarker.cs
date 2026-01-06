namespace ScottPlot.Plottables.Interactive;

public class InteractiveMarker : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public Cursor Cursor { get; set; } = Cursor.Hand;
    public MarkerStyle MarkerStyle { get; set; } = new(MarkerShape.FilledCircle, 15, Colors.Black);
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public double X;
    public double Y;
    public Coordinates Point { get => new(X, Y); set { X = value.X; Y = value.Y; } }

    public AxisLimits GetAxisLimits() => new(Point);

    public InteractiveHandle? GetHandle(CoordinateRect rect) =>
        rect.Contains(Point) ? new InteractiveHandle(this, Cursor) : null;

    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point) => Point = point;
    public virtual void PressHandle(InteractiveHandle handle) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }
    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        Pixel px = Axes.GetPixel(Point);
        MarkerStyle.Render(rp, px);
    }
}
