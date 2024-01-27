namespace ScottPlot.Plottables;

/// <summary>
/// A vertical span marks the full horizontal range between two vertical values
/// </summary>
public class VerticalSpan : AxisSpan, IPlottable
{
    public double Y1 { get; set; }
    public double Y2 { get; set; }

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
}
