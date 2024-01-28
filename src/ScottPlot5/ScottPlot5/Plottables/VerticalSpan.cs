namespace ScottPlot.Plottables;

/// <summary>
/// A vertical span marks the full horizontal range between two vertical values
/// </summary>
public class VerticalSpan : AxisSpan, IPlottable
{
    public double Y1 { get; set; }
    public double Y2 { get; set; }
    public CoordinateRange YRange => new(Bottom, Top);

    public double Bottom
    {
        get => Math.Min(Y1, Y2);
        set => Y1 = value;
    }

    public double Top
    {
        get => Math.Max(Y1, Y2);
        set => Y2 = value;
    }

    public override AxisLimits GetAxisLimits()
    {
        return AxisLimits.VerticalOnly(Bottom, Top);
    }

    public override void Render(RenderPack rp)
    {
        PixelRangeY vert = new(Axes.GetPixelY(Bottom), Axes.GetPixelY(Top));
        PixelRangeX horiz = new(rp.DataRect.Left, rp.DataRect.Right);
        PixelRect rect = new(horiz, vert);
        Render(rp, rect);
    }

    public override AxisSpanUnderMouse? UnderMouse(CoordinateRect rect)
    {
        AxisSpanUnderMouse spanUnderMouse = new()
        {
            Span = this,
            MouseStart = rect.Center,
            OriginalRange = new CoordinateRange(Y1, Y2),
        };

        if (IsResizable && rect.ContainsY(Y1))
        {
            spanUnderMouse.ResizeEdge1 = true;
            return spanUnderMouse;
        }
        else if (IsResizable && rect.ContainsY(Y2))
        {
            spanUnderMouse.ResizeEdge2 = true;
            return spanUnderMouse;
        }
        else if (IsDraggable && rect.YRange.Intersects(YRange))
        {
            return spanUnderMouse;
        }

        return null;
    }

    public override void DragTo(AxisSpanUnderMouse spanUnderMouse, Coordinates mouseNow)
    {
        if (spanUnderMouse.ResizeEdge1)
        {
            Y1 = mouseNow.Y;
        }
        else if (spanUnderMouse.ResizeEdge2)
        {
            Y2 = mouseNow.Y;
        }
        else
        {
            double dY = spanUnderMouse.MouseStart.Y - mouseNow.Y;
            Y1 = spanUnderMouse.OriginalRange.Min - dY;
            Y2 = spanUnderMouse.OriginalRange.Max - dY;
        }
    }
}
