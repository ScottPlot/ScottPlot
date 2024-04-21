namespace ScottPlot.Plottables;

/// <summary>
/// A line at a defined X position that spans the entire vertical space of the data area
/// </summary>
public class VerticalLine : AxisLine
{
    public double X
    {
        get => Position;
        set => Position = value;
    }

    public VerticalLine()
    {
        LabelStyle.ForeColor = Colors.White;
        LabelStyle.FontSize = 14;
        LabelStyle.Bold = true;
        LabelStyle.Padding = 5;
    }

    public override bool IsUnderMouse(CoordinateRect rect) => IsDraggable && rect.ContainsX(X);

    public override AxisLimits GetAxisLimits()
    {
        return AxisLimits.HorizontalOnly(X, X);
    }

    public override void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        // determine location
        float y1 = rp.DataRect.Bottom;
        float y2 = rp.DataRect.Top;
        float x = Axes.GetPixelX(X);

        // do not render if the axis line is outside the data area
        if (!rp.DataRect.ContainsX(x))
            return;

        // draw line inside the data area
        PixelLine line = new(x, y1, x, y2);

        using SKPaint paint = new();
        LineStyle.Render(rp.Canvas, line, paint);
    }

    public override void RenderLast(RenderPack rp)
    {
        if (LabelStyle.IsVisible == false || string.IsNullOrEmpty(LabelStyle.Text))
            return;

        // determine location
        float x = Axes.GetPixelX(X);

        // do not render if the axis line is outside the data area
        if (!rp.DataRect.ContainsX(x))
            return;

        float y = LabelOppositeAxis
            ? rp.DataRect.Top - LabelStyle.PixelPadding.Top
            : rp.DataRect.Bottom + LabelStyle.PixelPadding.Bottom;

        Alignment defaultAlignment = LabelOppositeAxis
            ? Alignment.LowerCenter
            : Alignment.UpperCenter;

        LabelStyle.Alignment = ManualLabelAlignment ?? defaultAlignment;

        // draw label outside the data area
        rp.CanvasState.DisableClipping();

        using SKPaint paint = new();
        LabelStyle.Render(rp.Canvas, new Pixel(x, y), paint);
    }
}
