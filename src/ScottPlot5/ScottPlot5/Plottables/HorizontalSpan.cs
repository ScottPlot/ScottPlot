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
}
