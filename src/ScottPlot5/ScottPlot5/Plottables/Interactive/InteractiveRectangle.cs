namespace ScottPlot.Plottables.Interactive;

public class InteractiveRectangle : IPlottable, IHasInteractiveHandles
{
    private Coordinates DragStartPoint;
    private CoordinateRect DragBaseRect;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public LineStyle LineStyle { get; } = new(2, Colors.Black);
    public FillStyle FillStyle { get; } = new(Colors.Black.WithOpacity(.2));

    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }
    public double XMin => Math.Min(X1, X2);
    public double XMax => Math.Max(X1, X2);
    public double YMin => Math.Min(Y1, Y2);
    public double YMax => Math.Max(Y1, Y2);

    public CoordinateRect Rect
    {
        get => new(XMin, XMax, YMin, YMax);
        set
        {
            X1 = value.Left;
            X2 = value.Right;
            Y1 = value.Top;
            Y2 = value.Bottom;
        }
    }

    public AxisLimits GetAxisLimits() => new(Rect);

    enum Handles { X1, Y1, X2, Y2, Body };

    public InteractiveHandle? GetHandle(CoordinateRect rect)
    {
        bool inVerticalRange = rect.Center.Y >= YMin && rect.Center.Y <= YMax;
        bool inHorizontalRange = rect.Center.X >= XMin && rect.Center.X <= XMax;

        if (rect.ContainsX(X1) && inVerticalRange)
        {
            return new InteractiveHandle(this, Cursor.SizeWestEast, (int)Handles.X1);
        }

        if (rect.ContainsX(X2) && inVerticalRange)
        {
            return new InteractiveHandle(this, Cursor.SizeWestEast, (int)Handles.X2);
        }

        if (rect.ContainsY(Y1) && inHorizontalRange)
        {
            return new InteractiveHandle(this, Cursor.SizeNorthSouth, (int)Handles.Y1);
        }

        if (rect.ContainsY(Y2) && inHorizontalRange)
        {
            return new InteractiveHandle(this, Cursor.SizeNorthSouth, (int)Handles.Y2);
        }

        if (inVerticalRange && inHorizontalRange)
        {
            return new InteractiveHandle(this, Cursor.SizeAll, (int)Handles.Body);
        }

        return null;
    }

    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point)
    {
        if (handle.Index == (int)Handles.X1)
        {
            X1 = point.X;
        }
        else if (handle.Index == (int)Handles.X2)
        {
            X2 = point.X;
        }
        if (handle.Index == (int)Handles.Y1)
        {
            Y1 = point.Y;
        }
        else if (handle.Index == (int)Handles.Y2)
        {
            Y2 = point.Y;
        }
        else if (handle.Index == (int)Handles.Body)
        {
            var deltaX = point.X - DragStartPoint.X;
            var deltaY = point.Y - DragStartPoint.Y;
            double x1 = DragBaseRect.Left + deltaX;
            double x2 = DragBaseRect.Right + deltaX;
            double y1 = DragBaseRect.Bottom + deltaY;
            double y2 = DragBaseRect.Top + deltaY;
            Rect = new(x1, x2, y1, y2);
        }
    }

    public virtual void PressHandle(InteractiveHandle handle, Coordinates point)
    {
        DragStartPoint = point;
        DragBaseRect = Rect;
    }

    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        PixelRect rect = Axes.GetPixelRect(Rect);
        FillStyle.Render(rp, rect);
        LineStyle.Render(rp, rect);
    }
}
