namespace ScottPlot.Plottables;

/// <summary>
/// A line at a defined Y position that spans the entire horizontal space of the data area
/// </summary>
public class HorizontalLine : AxisLine
{
    public double Y
    {
        get => Position;
        set => Position = value;
    }

    public HorizontalLine()
    {
        LabelStyle.Rotation = -90;
        LabelStyle.Alignment = Alignment.LowerCenter;
        LabelStyle.FontSize = 14;
        LabelStyle.Bold = true;
        LabelStyle.ForeColor = Colors.White;
        LabelStyle.Padding = 5;
    }

    public override bool IsUnderMouse(CoordinateRect rect) => IsDraggable && rect.ContainsY(Y);

    public override AxisLimits GetAxisLimits()
    {
        return AxisLimits.VerticalOnly(Y, Y);
    }

    public override void Render(RenderPack rp)
    {
        if (!IsVisible)
            return;

        // determine location
        float x1 = rp.DataRect.Left;
        float x2 = rp.DataRect.Right;
        float y = Axes.GetPixelY(Y);

        // do not render if the axis line is outside the data area
        if (!rp.DataRect.ContainsY(y))
            return;

        // draw line inside the data area
        PixelLine line = new(x1, y, x2, y);
        using SKPaint paint = new();
        LineStyle.Render(rp.Canvas, line, paint);
    }

    public override void RenderLast(RenderPack rp)
    {
        if (LabelStyle.IsVisible == false || string.IsNullOrEmpty(LabelStyle.Text))
            return;

        // determine location
        float y = Axes.GetPixelY(Y);

        // do not render if the axis line is outside the data area
        if (!rp.DataRect.ContainsY(y))
            return;

        float x = LabelOppositeAxis
            ? rp.DataRect.Right + LabelStyle.PixelPadding.Right
            : rp.DataRect.Left - LabelStyle.PixelPadding.Left;

        Alignment defaultAlignment = LabelOppositeAxis
            ? Alignment.UpperCenter
            : Alignment.LowerCenter;

        LabelStyle.Alignment = ManualLabelAlignment ?? defaultAlignment;

        // draw label outside the data area
        rp.CanvasState.DisableClipping();

        using SKPaint paint = new();
        LabelStyle.Render(rp.Canvas, new Pixel(x, y), paint);
    }
}
