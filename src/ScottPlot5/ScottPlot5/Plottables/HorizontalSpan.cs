namespace ScottPlot.Plottables;

/// <summary>
/// A horizontal span marks the full vertical range between two horizontal values
/// </summary>
public class HorizontalSpan : AxisSpan, IPlottable
{
    public double X1 { get; set; }
    public double X2 { get; set; }
    public CoordinateRange XRange => new(Left, Right);

    public double Left
    {
        get => Math.Min(X1, X2);
        set => X1 = value;
    }

    public double Right
    {
        get => Math.Max(X1, X2);
        set => X2 = value;
    }

    public override AxisLimits GetAxisLimits()
    {
        return AxisLimits.HorizontalOnly(Left, Right);
    }

    public override void Render(RenderPack rp)
    {
        PixelRangeY vert = new(rp.DataRect.Bottom, rp.DataRect.Top);
        PixelRangeX horiz = new(Axes.GetPixelX(Left), Axes.GetPixelX(Right));
        PixelRect rect = new(horiz, vert);
        Render(rp, rect);
    }

    public override AxisSpanUnderMouse? UnderMouse(CoordinateRect rect)
    {
        AxisSpanUnderMouse spanUnderMouse = new()
        {
            Span = this,
            MouseStart = rect.Center,
            OriginalRange = new CoordinateRange(X1, X2),
        };

        if (IsResizable && rect.ContainsX(X1))
        {
            spanUnderMouse.ResizeEdge1 = true;
            return spanUnderMouse;
        }
        else if (IsResizable && rect.ContainsX(X2))
        {
            spanUnderMouse.ResizeEdge2 = true;
            return spanUnderMouse;
        }
        else if (IsDraggable && rect.XRange.Intersects(XRange))
        {
            return spanUnderMouse;
        }

        return null;
    }

    public override void DragTo(AxisSpanUnderMouse spanUnderMouse, Coordinates mouseNow)
    {
        if (spanUnderMouse.ResizeEdge1)
        {
            X1 = mouseNow.X;
        }
        else if (spanUnderMouse.ResizeEdge2)
        {
            X2 = mouseNow.X;
        }
        else
        {
            double dX = spanUnderMouse.MouseStart.X - mouseNow.X;
            X1 = spanUnderMouse.OriginalRange.Min - dX;
            X2 = spanUnderMouse.OriginalRange.Max - dX;
        }
    }
}
