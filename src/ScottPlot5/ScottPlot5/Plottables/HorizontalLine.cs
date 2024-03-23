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
        Label.Rotation = -90;
        Label.Alignment = Alignment.LowerCenter;
        Label.FontSize = 14;
        Label.Bold = true;
        Label.ForeColor = Colors.White;
        Label.Padding = 5;
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
        LineStyle.Render(rp.Canvas, paint, line);
    }

    public override void RenderLast(RenderPack rp)
    {
        if (Label.IsVisible == false || string.IsNullOrEmpty(Label.Text))
            return;

        // determine location
        float y = Axes.GetPixelY(Y);

        // do not render if the axis line is outside the data area
        if (!rp.DataRect.ContainsY(y))
            return;

        float x = LabelOppositeAxis
            ? rp.DataRect.Right + Label.Padding
            : rp.DataRect.Left - Label.Padding;

        Label.Alignment = LabelOppositeAxis
            ? Alignment.UpperCenter
            : Alignment.LowerCenter;

        using SKPaint paint = new();
        Label.Render(rp.Canvas, x, y, paint);
    }
}
