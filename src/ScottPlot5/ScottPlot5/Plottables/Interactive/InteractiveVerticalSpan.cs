namespace ScottPlot.Plottables.Interactive;

public class InteractiveVerticalSpan : IPlottable, IHasInteractiveHandles
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public object? Tag { get; set; }
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public Cursor EdgeCursor { get; set; } = Cursor.SizeNorthSouth;
    public Cursor BodyCursor { get; set; } = Cursor.SizeAll;

    public LineStyle LineStyle { get; } = new(2, Colors.Black);
    public FillStyle FillStyle { get; } = new(Colors.Black.WithOpacity(.2));

    public double Y1 { get; set; }
    public double Y2 { get; set; }
    public double YMin => Math.Min(Y1, Y2);
    public double YMax => Math.Max(Y1, Y2);

    public AxisLimits GetAxisLimits() => AxisLimits.VerticalOnly(YMin, YMax);

    enum Handles { Edge1, Body, Edge2 };

    public InteractiveHandle? GetHandle(CoordinateRect rect)
    {
        if (rect.ContainsY(Y1))
        {
            return new InteractiveHandle(this, EdgeCursor, (int)Handles.Edge1);
        }

        if (rect.ContainsY(Y2))
        {
            return new InteractiveHandle(this, EdgeCursor, (int)Handles.Edge2);
        }

        if (rect.Center.Y >= YMin && rect.Center.Y <= YMax)
        {
            return new InteractiveHandle(this, BodyCursor, (int)Handles.Body);
        }

        return null;
    }

    public virtual void MoveHandle(InteractiveHandle handle, Coordinates point)
    {
        if (handle.Index == (int)Handles.Edge1)
        {
            Y1 = point.Y;
        }
        else if (handle.Index == (int)Handles.Edge2)
        {
            Y2 = point.Y;
        }
        else if (handle.Index == (int)Handles.Body)
        {
            double span = Math.Abs(Y1 - Y2);
            Y1 = point.Y - span / 2;
            Y2 = point.Y + span / 2;
        }
    }

    public virtual void PressHandle(InteractiveHandle handle) { }

    public virtual void ReleaseHandle(InteractiveHandle handle) { }

    public virtual void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        float yMin = Axes.GetPixelY(YMin);
        float yMax = Axes.GetPixelY(YMax);

        PixelRect rect = new(rp.DataRect.Left, rp.DataRect.Right, yMin, yMax);
        FillStyle.Render(rp, rect);

        PixelLine line1 = new(rp.DataRect.Left, yMin, rp.DataRect.Right, yMin);
        PixelLine line2 = new(rp.DataRect.Left, yMax, rp.DataRect.Right, yMax);
        LineStyle.Render(rp, line1);
        LineStyle.Render(rp, line2);
    }
}
