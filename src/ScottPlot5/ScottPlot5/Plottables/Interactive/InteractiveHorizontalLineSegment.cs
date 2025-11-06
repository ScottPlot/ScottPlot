namespace ScottPlot.Plottables.Interactive;

public class InteractiveHorizontalLineSegment : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public LineStyle LineStyle { get; } = new(2, Colors.Black);
    public float HandleLength { get; set; } = 10;

    public double Y { get; set; }
    public double X1 { get; set; }
    public double X2 { get; set; }
    public double XMin => Math.Min(X1, X2);
    public double XMax => Math.Max(X1, X2);

    public AxisLimits GetAxisLimits() => new(XMin, XMax, Y, Y);

    enum Handle { Edge1, Body, Edge2 };

    public InteractiveHandle? GetHandle(CoordinateRect rect)
    {
        if (rect.ContainsY(Y) == false)
            return null;

        if (rect.ContainsX(X1))
            return new InteractiveHandle(this, Cursor.SizeWestEast, (int)Handle.Edge1);

        if (rect.ContainsX(X2))
            return new InteractiveHandle(this, Cursor.SizeWestEast, (int)Handle.Edge2);

        if (rect.Center.X >= XMin && rect.Center.X <= XMax)
            return new InteractiveHandle(this, Cursor.SizeNorthSouth, (int)Handle.Body);

        return null;
    }

    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point)
    {
        if (handle.Index == (int)Handle.Edge1)
        {
            X1 = point.X;
        }
        else if (handle.Index == (int)Handle.Edge2)
        {
            X2 = point.X;
        }
        else if (handle.Index == (int)Handle.Body)
        {
            Y = point.Y;
        }
    }

    public virtual void PressHandle(InteractiveHandle handle, Coordinates point) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        float y = Axes.GetPixelY(Y);
        float yBelow = y + HandleLength / 2;
        float yAbove = y - HandleLength / 2;
        float x1 = Axes.GetPixelX(XMin);
        float x2 = Axes.GetPixelX(XMax);

        PixelLine lineBody = new(x1, y, x2, y);
        PixelLine lineLeft = new(x1, yBelow, x1, yAbove);
        PixelLine lineRight = new(x2, yBelow, x2, yAbove);

        LineStyle.Render(rp, lineBody);
        LineStyle.Render(rp, lineLeft);
        LineStyle.Render(rp, lineRight);
    }
}
