namespace ScottPlot.Plottables.Interactive;

public class InteractiveHorizontalSpan : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public Cursor EdgeCursor { get; set; } = Cursor.SizeWestEast;
    public Cursor BodyCursor { get; set; } = Cursor.SizeAll;

    public LineStyle LineStyle { get; } = new(2, Colors.Black);
    public FillStyle FillStyle { get; } = new(Colors.Black.WithOpacity(.2));

    public double X1 { get; set; }
    public double X2 { get; set; }
    public double XMin => Math.Min(X1, X2);
    public double XMax => Math.Max(X1, X2);

    public AxisLimits GetAxisLimits() => AxisLimits.HorizontalOnly(XMin, XMax);

    enum Handles { Edge1, Body, Edge2 };

    public InteractiveHandle? GetHandle(CoordinateRect rect)
    {
        if (rect.ContainsX(X1))
        {
            return new InteractiveHandle(this, EdgeCursor, (int)Handles.Edge1);
        }

        if (rect.ContainsX(X2))
        {
            return new InteractiveHandle(this, EdgeCursor, (int)Handles.Edge2);
        }

        if (rect.Center.X >= XMin && rect.Center.X <= XMax)
        {
            return new InteractiveHandle(this, BodyCursor, (int)Handles.Body);
        }

        return null;
    }

    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point)
    {
        if (handle.Index == (int)Handles.Edge1)
        {
            X1 = point.X;
        }
        else if (handle.Index == (int)Handles.Edge2)
        {
            X2 = point.X;
        }
        else if (handle.Index == (int)Handles.Body)
        {
            double span = Math.Abs(X1 - X2);
            X1 = point.X - span / 2;
            X2 = point.X + span / 2;
        }
    }

    public virtual void PressHandle(InteractiveHandle handle) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        float xMin = Axes.GetPixelX(XMin);
        float xMax = Axes.GetPixelX(XMax);

        PixelRect rect = new(xMin, xMax, rp.DataRect.Bottom, rp.DataRect.Top);
        FillStyle.Render(rp, rect);

        PixelLine line1 = new(xMin, rp.DataRect.Bottom, xMin, rp.DataRect.Top);
        PixelLine line2 = new(xMax, rp.DataRect.Bottom, xMax, rp.DataRect.Top);
        LineStyle.Render(rp, line1);
        LineStyle.Render(rp, line2);
    }
}

