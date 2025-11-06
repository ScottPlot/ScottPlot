namespace ScottPlot.Plottables.Interactive;

/// <summary>
/// A straight line between two points
/// </summary>
public class InteractiveLineSegment : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();

    public Cursor Cursor { get; set; } = Cursor.Hand;

    private MutableCoordinateLine MutableLine { get; } = new();
    public CoordinateLine Line
    {
        get => MutableLine.CoordinateLine;
        set => MutableLine.Update(value);
    }

    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => MutableLine.AxisLimits;

    public LineStyle LineStyle { get; } = new LineStyle(2);

    public MarkerStyle StartMarkerStyle { get; } = new(MarkerShape.FilledCircle, 8);
    public MarkerStyle EndMarkerStyle { get; } = new(MarkerShape.FilledCircle, 8);

    enum Node { Point1, Point2 };

    public InteractiveHandle? GetHandle(CoordinateRect rect)
    {
        if (rect.Contains(MutableLine.Point1))
            return new InteractiveHandle(this, Cursor, (int)Node.Point1);

        if (rect.Contains(MutableLine.Point2))
            return new InteractiveHandle(this, Cursor, (int)Node.Point2);

        return null;
    }

    public Color Color
    {
        set
        {
            LineStyle.Color = value;
            StartMarkerStyle.FillColor = value;
            EndMarkerStyle.FillColor = value;
        }
    }

    public virtual void PressHandle(InteractiveHandle handle, Coordinates point) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }
    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point)
    {
        if (handle.Index == (int)Node.Point1)
            MutableLine.Point1 = point;

        if (handle.Index == (int)Node.Point2)
            MutableLine.Point2 = point;
    }

    public virtual void Render(RenderPack rp)
    {
        if (IsVisible == false)
            return;

        PixelLine pxLine = Axes.GetPixelLine(MutableLine.CoordinateLine);
        LineStyle.Render(rp.Canvas, pxLine, rp.Paint);
        StartMarkerStyle.Render(rp.Canvas, pxLine.Pixel1, rp.Paint);
        EndMarkerStyle.Render(rp.Canvas, pxLine.Pixel2, rp.Paint);
    }
}
