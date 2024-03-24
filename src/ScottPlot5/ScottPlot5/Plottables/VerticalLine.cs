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
        Label.ForeColor = Colors.White;
        Label.FontSize = 14;
        Label.Bold = true;
        Label.Padding = 5;
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
        LineStyle.Render(rp.Canvas, paint, line);
    }

    public override void RenderLast(RenderPack rp)
    {
        if (Label.IsVisible == false || string.IsNullOrEmpty(Label.Text))
            return;

        // determine location
        float x = Axes.GetPixelX(X);

        // do not render if the axis line is outside the data area
        if (!rp.DataRect.ContainsX(x))
            return;

        float y = LabelOppositeAxis
            ? rp.DataRect.Top - Label.Padding
            : rp.DataRect.Bottom + Label.Padding;

        Label.Alignment = LabelOppositeAxis
            ? Alignment.LowerCenter
            : Alignment.UpperCenter;

        // draw label outside the data area
        rp.CanvasState.DisableClipping();

        using SKPaint paint = new();
        Label.Render(rp.Canvas, x, y, paint);
    }
}
