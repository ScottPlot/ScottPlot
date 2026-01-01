namespace ScottPlot.Plottables.Interactive;

public class InteractiveVerticalLineSegment : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public LineStyle LineStyle { get; } = new(2, Colors.Black);
    public float HandleLength { get; set; } = 10;

    public double X { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }
    public double YMin => Math.Min(Y1, Y2);
    public double YMax => Math.Max(Y1, Y2);

    public AxisLimits GetAxisLimits() => new(X, X, YMin, YMax);

    enum Handle { Edge1, Body, Edge2 };

    public InteractiveHandle? GetHandle(CoordinateRect rect)
    {
        if (rect.ContainsX(X) == false)
            return null;

        if (rect.ContainsY(Y1))
            return new InteractiveHandle(this, Cursor.SizeNorthSouth, (int)Handle.Edge1);

        if (rect.ContainsY(Y2))
            return new InteractiveHandle(this, Cursor.SizeNorthSouth, (int)Handle.Edge2);

        if (rect.Center.Y >= YMin && rect.Center.Y <= YMax)
            return new InteractiveHandle(this, Cursor.SizeWestEast, (int)Handle.Body);

        return null;
    }

    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point)
    {
        if (handle.Index == (int)Handle.Edge1)
        {
            Y1 = point.Y;
        }
        else if (handle.Index == (int)Handle.Edge2)
        {
            Y2 = point.Y;
        }
        else if (handle.Index == (int)Handle.Body)
        {
            X = point.X;
        }
    }

    public virtual void PressHandle(InteractiveHandle handle) { }
    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        float x = Axes.GetPixelX(X);
        float xBelow = x - HandleLength / 2;
        float xAbove = x + HandleLength / 2;
        float y1 = Axes.GetPixelY(YMin);
        float y2 = Axes.GetPixelY(YMax);

        PixelLine lineBody = new(x, y1, x, y2);
        PixelLine lineLeft = new(xBelow, y1, xAbove, y1);
        PixelLine lineRight = new(xBelow, y2, xAbove, y2);

        LineStyle.Render(rp, lineBody);
        LineStyle.Render(rp, lineLeft);
        LineStyle.Render(rp, lineRight);
    }
}
