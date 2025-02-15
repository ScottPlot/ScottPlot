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

    public double Minimum { get; set; } = double.NegativeInfinity;
    public double Maximum { get; set; } = double.PositiveInfinity;

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
        return EnableAutoscale ? AxisLimits.VerticalOnly(Y, Y) : AxisLimits.NoLimits;
    }

    public override void Render(RenderPack rp)
    {
        if (!IsVisible || !Axes.YAxis.Range.Contains(Y))
            return;

        Coordinates pt1 = new(Math.Max(Minimum, Axes.XAxis.Min), Y);
        Coordinates pt2 = new(Math.Min(Maximum, Axes.XAxis.Max), Y);
        CoordinateLine line = new(pt1, pt2);
        PixelLine pxLine = Axes.GetPixelLine(line);
        LineStyle.Render(rp.Canvas, pxLine, rp.Paint);
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
            : rp.DataRect.Left - LabelStyle.PixelPadding.Left - rp.Layout.PanelOffsets[Axes.YAxis];

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
